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
    public class MoviesService : IMoviesService
    {
        private readonly ILogger<MoviesService> _logger;
        private readonly HttpClient _httpClient;
        private readonly OmdbApiSettings _omdbApiSettings;

        private readonly IValidatorFactory _validatorFactory;
        private readonly IMovieRespository _movieRepository;
        private readonly ICacheProvider<MovieResult> _cacheProvider;

        public MoviesService(ILogger<MoviesService> logger, IOptions<OmdbApiSettings> omdbApiOptions, IHttpClientFactory httpClientFactory, IValidatorFactory validatorFactory, IMovieRespository movieRepository, ICacheProvider<MovieResult> cacheProvider)
        {
            _logger = logger;
            _omdbApiSettings = omdbApiOptions.Value;
            _httpClient = httpClientFactory.CreateClient(StringConstants.OmdbApiClientName);
            _validatorFactory = validatorFactory;
            _movieRepository = movieRepository;
            _cacheProvider = cacheProvider;
        }

        public async Task<OneOf<GetMovieByIdResult, ErrorResult>> GetMovieByIdAsync(GetMovieByIdRequest request)
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
                var movieResponse = JsonSerializer.Deserialize<GetMovieResponse>(await response.Content.ReadAsStringAsync());
                if (movieResponse == null || !movieResponse.IsNot404())
                {
                    return ErrorResult.FromError(404, new[] { movieResponse.Error });
                }

                var movie = movieResponse.Adapt<MovieResult>();

                // Save to the database and add to the cache.
                await _movieRepository.SaveOrUpdateAsync(movieResponse.Adapt<Movie>());
                _cacheProvider.AddOrUpdate(movieResponse.IMDbID, movie);

                return new GetMovieByIdResult(movie);
            }

            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get movie by IMDb ID: {IMDbID}.", request.IMDbID);
                return ErrorResult.FromError(500, new[] { $"An unexpected error occurred while attempting to get movie by IMDb ID: {request.IMDbID}." });
            }
        }

        public async Task<OneOf<GetMovieByTitleResult, ErrorResult>> GetMovieByTitleAsync(GetMovieByTitleRequest request)
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

                var movies = movieResponse.Search.Adapt<IEnumerable<MovieResult>>();

                // Save to the database and add to the cache.
                foreach (var movie in movieResponse.Search)
                {
                    await _movieRepository.SaveOrUpdateAsync(movie.Adapt<Movie>());
                    _cacheProvider.AddOrUpdate(movie.IMDbID, movies.SingleOrDefault(m => m.IMDbID.Equals(movie.IMDbID)));
                }

                return new GetMovieByTitleResult(movies);
            }

            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get movie by title.", request.Title);
                return ErrorResult.FromError(500, new[] { $"An unexpected error occurred while attempting to get movie by title: {request.Title}." });
            }
        }

        public IEnumerable<MovieResult> GetCachedEntries()
        {
            try
            {
                return _cacheProvider.GetAllItems();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred while attempting to get all cached entries.");
                return Array.Empty<MovieResult>();
            }            
        }

        private async Task<ValidationResult> ValidateRequestAsync<TRequest>(TRequest request)
        {
            var validator = _validatorFactory.GetValidator<TRequest>();
            return await validator.ValidateAsync(request);
        }

        public Task CreateMovieAsync(CreateMovieRequest request)
        {
            throw new NotImplementedException();
        }
    }
}