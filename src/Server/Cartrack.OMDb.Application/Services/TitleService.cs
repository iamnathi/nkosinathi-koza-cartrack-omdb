using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Application.Configurations;
using Cartrack.OMDb.Domain.Models;
using Cartrack.OMDb.Repositories;
using Cartrack.OMDb.Web.Models.Omdb;
using Cartrack.OMDb.Web.Models.Requests;
using Cartrack.OMDb.Web.Models.Results;
using Cartrack.OMDb.Web.Models.Results.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Application.Services
{
    public class TitleService : ITitleService
    {
        private readonly ILogger<TitleService> _logger;
        private readonly HttpClient _httpClient;
        private readonly OmdbApiSettings _omdbApiSettings;

        private readonly IValidatorFactory _validatorFactory;
        private readonly ITitleRespository _titleRepository;
        private readonly ICacheProvider<TitleResult> _cacheProvider;

        public TitleService(ILogger<TitleService> logger, IOptions<OmdbApiSettings> omdbApiOptions, IHttpClientFactory httpClientFactory, IValidatorFactory validatorFactory, ITitleRespository movieRepository, ICacheProvider<TitleResult> cacheProvider)
        {
            _logger = logger;
            _omdbApiSettings = omdbApiOptions.Value;
            _httpClient = httpClientFactory.CreateClient(StringConstants.OmdbApiClientName);
            _validatorFactory = validatorFactory;
            _titleRepository = movieRepository;
            _cacheProvider = cacheProvider;
        }

        public async Task<OneOf<GetTitleResult, ErrorResult>> GetTitleByIdAsync(GetTitleByIdRequest request)
        {
            try
            {
                var validation = await ValidateRequestAsync(request);
                if (!validation.IsValid)
                {
                    return ErrorResult.FromError(400, validation.Errors.Select(p => p.ErrorMessage).ToList());
                }

                var response = await _httpClient.GetAsync(string.Format(StringConstants.OmdbApiQueryStringTemplate, _omdbApiSettings.ApiKey, request));
                if (!response.IsSuccessStatusCode)
                {
                    return ErrorResult.FromError((int)response.StatusCode, new[] { response.ReasonPhrase });
                }

                // The OMDb API returns 200 whether the movie was found or not
                var movieResponse = JsonSerializer.Deserialize<GetTitleResponse>(await response.Content.ReadAsStringAsync());
                if (movieResponse == null || !movieResponse.IsNot404())
                {
                    return ErrorResult.FromError(404, new[] { movieResponse.Error });
                }

                var movie = movieResponse.Adapt<TitleResult>();

                // Save to the database and add to the cache.
                await _titleRepository.SaveOrUpdateAsync(movieResponse.Adapt<TitleModel>());
                _cacheProvider.AddOrUpdate(movieResponse.IMDbID, movie);

                return new GetTitleResult(movie);
            }

            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get movie by IMDb ID: {IMDbID}.", request.IMDbID);
                return ErrorResult.FromError(500, new[] { $"An unexpected error occurred while attempting to get movie by IMDb ID: {request.IMDbID}." });
            }
        }

        public async Task<OneOf<SearchTitlesResult, ErrorResult>> SearchTitlesAsync(SearchTitlesRequest request)
        {
            try
            {
                var validation = await ValidateRequestAsync(request);
                if (!validation.IsValid)
                {
                    return ErrorResult.FromError(400, validation.Errors.Select(p => p.ErrorMessage).ToList());
                }

                var response = await _httpClient.GetAsync(string.Format(StringConstants.OmdbApiQueryStringTemplate, _omdbApiSettings.ApiKey, request));
                if (!response.IsSuccessStatusCode)
                {
                    return ErrorResult.FromError((int)response.StatusCode, new[] { response.ReasonPhrase });
                }

                // The OMDb API returns 200 whether the movie was found or not
                var movieResponse = JsonSerializer.Deserialize<SearchMovieResponse>(await response.Content.ReadAsStringAsync());
                if (movieResponse == null || !movieResponse.IsNot404())
                {
                    return ErrorResult.FromError(404, new[] { movieResponse.Error });
                }

                var movies = movieResponse.Search.Adapt<IEnumerable<TitleResult>>();

                // Save to the database and add to the cache.
                foreach (var movie in movieResponse.Search)
                {
                    await _titleRepository.SaveOrUpdateAsync(movie.Adapt<TitleModel>());
                    _cacheProvider.AddOrUpdate(movie.IMDbID, movies.SingleOrDefault(m => m.IMDbID.Equals(movie.IMDbID)));
                }

                return new SearchTitlesResult(movies, movieResponse.TotalSearchCount, request.PageNumber);
            }

            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get movie by title.", request.SearchTerm);
                return ErrorResult.FromError(500, new[] { $"An unexpected error occurred while attempting to get movie by title: {request.SearchTerm}." });
            }
        }

        public async Task<OneOf<GetTitleResult, ErrorResult>> SaveOrUpdateTitleAsync(CreateOrUpdateTitleRequest request, bool isUpdateRequest = false)
        {
            try
            {
                var validation = await ValidateRequestAsync(request);
                if (!validation.IsValid)
                {
                    return ErrorResult.FromError(400, validation.Errors.Select(p => p.ErrorMessage).ToList());
                }

                if (isUpdateRequest && !_cacheProvider.TryGetValue(request.IMDbID, out var cache))
                {
                    return ErrorResult.FromError(404, new[] { "Movie, series, or episode not found." });
                }

                await _titleRepository.SaveOrUpdateAsync(request.Adapt<TitleModel>());

                cache = request.Adapt<TitleResult>();
                _cacheProvider.AddOrUpdate(cache.IMDbID, cache);

                return new GetTitleResult(cache);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to create a movie, series, or episode.");
                return ErrorResult.FromError(500, new[] { "An unexpected error occurred while attempting to create a movie, series, or episode." });
            }
        }

        public IEnumerable<TitleResult> GetCachedTitles()
        {
            try
            {
                return _cacheProvider.GetAllItems();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get all cached entries.");
                return Array.Empty<TitleResult>();
            }            
        }

        public async Task<bool> DeleteTitleAsync(string imdbId)
        {
            try
            {
                await _titleRepository.DeleteAsync(imdbId);
                _cacheProvider.Delete(imdbId);
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to delete a movie, series, or episode.");
                return false;
            }
        }

        private async Task<ValidationResult> ValidateRequestAsync<TRequest>(TRequest request)
        {
            var validator = _validatorFactory.GetValidator<TRequest>();
            return await validator.ValidateAsync(request);
        }
    }
}