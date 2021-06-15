using Cartrack.OMDb.Clients.Cli.API;
using Cartrack.OMDb.Clients.Cli.Models;
using System.Threading.Tasks;
using Mapster;
using System;
using FluentValidation;
using OneOf;
using System.Linq;
using Refit;
using System.Net;
using Cartrack.OMDb.Clients.Cli.API.Requests;

namespace Cartrack.OMDb.Clients.Cli.Services
{
    public class TitlesService : ITitlesService
    {
        private readonly ICartrackOMDbApi _titlesApi;

        private readonly IValidator<TitleRequest> _getTitleValidator;
        private readonly IValidator<SearchTitlesRequest> _searchTitleValidator;
        private readonly IValidator<CreateOrUpdateTitleRequest> _createOrUpdateTitleValidator;

        public TitlesService(ICartrackOMDbApi titlesApi, IValidator<TitleRequest> getTitleValidator, 
            IValidator<SearchTitlesRequest> searchTitleValidator, IValidator<CreateOrUpdateTitleRequest> createOrUpdateTitleValidator)
        {
            _titlesApi = titlesApi;

            _getTitleValidator = getTitleValidator;
            _searchTitleValidator = searchTitleValidator;
            _createOrUpdateTitleValidator = createOrUpdateTitleValidator;
        }

        public async Task<OneOf<GetTitleResult, TitleErrorResult>> GetTitleByIMDbIdAsync(TitleRequest request)
        {
            return await HandleError<GetTitleResult>(async () =>
            {
                var validation = await _getTitleValidator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var messages = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return new TitleErrorResult(messages);
                }

                var response = await _titlesApi.GetTitlesByIdAsync(request.IMDbID);
                return response.Adapt<GetTitleResult>();
            });           
        }

        public async Task<OneOf<SearchTitlesResult, TitleErrorResult>> SearchTitlesAsync(SearchTitlesRequest request)
        {
            return await HandleError<SearchTitlesResult>(async () =>
            {
                var validation = await _searchTitleValidator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var messages = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return new TitleErrorResult(messages);
                }

                var response = await _titlesApi.SearchTitlesAsync(request.SearchTerm, request.Year);
                return response.Adapt<SearchTitlesResult>();
            });            
        }

        public async Task<OneOf<dynamic, TitleErrorResult>> QueryCacheEntryAsync(QueryTitlesRequest request)
        {
            return await HandleError<dynamic>(async () =>
            {
                var response = await _titlesApi.QueryCachedEntriesAsync(new ODataQueryParams(request.Select, request.Where, request.OrderBy, request.Count));
                return response;
            });
        }

        public async Task<OneOf<CreateOrUpdateTitleResult, TitleErrorResult>> CreateCacheEntryAsync(CreateOrUpdateTitleRequest request)
        {
            return await HandleError<CreateOrUpdateTitleResult>(async () =>
            {
                var validation = await _createOrUpdateTitleValidator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var messages = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return new TitleErrorResult(messages);
                }

                var response = await _titlesApi.CreateCachedEntryAsync(request.Adapt<CreateOrUpdateCacheEntryRequest>());
                return response.Adapt<CreateOrUpdateTitleResult>();
            });
        }

        public async Task<OneOf<CreateOrUpdateTitleResult, TitleErrorResult>> UpdateCacheEntryAsync(CreateOrUpdateTitleRequest request)
        {
            return await HandleError<CreateOrUpdateTitleResult>(async () =>
            {
                var validation = await _createOrUpdateTitleValidator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var messages = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return new TitleErrorResult(messages);
                }

                var response = await _titlesApi.UpdateCachedEntryAsync(request.IMDbID, request.Adapt<CreateOrUpdateCacheEntryRequest>());
                return response.Adapt<CreateOrUpdateTitleResult>();
            });
        }

        public async Task<OneOf<bool, TitleErrorResult>> DeleteCacheEntryAsync(TitleRequest request)
        {
            return await HandleError<bool>(async () =>
            {
                var validation = await _getTitleValidator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var messages = validation.Errors.Select(e => e.ErrorMessage).ToList();
                    return new TitleErrorResult(messages);
                }

                await _titlesApi.DeleteCachedEntryAsync(request.IMDbID);
                return true;
            });
        }

        private async Task<OneOf<TResult, TitleErrorResult>> HandleError<TResult>(Func<Task<OneOf<TResult, TitleErrorResult>>> func)
        {
            try
            {
                return await func();
            }
            catch (ApiException exception)
            {
                switch (exception.StatusCode)
                {

                    case HttpStatusCode.NotFound:
                        return new TitleErrorResult(new[] { "Movie not found." });
                    default:
                        return new TitleErrorResult(new[] { exception.ReasonPhrase });
                }
            }
            catch (Exception exception)
            {
                return new TitleErrorResult(new[] { exception.Message });
            }
        }
    }
}