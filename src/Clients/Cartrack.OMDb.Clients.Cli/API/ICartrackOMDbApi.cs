using Cartrack.OMDb.Clients.Cli.API.Requests;
using Cartrack.OMDb.Clients.Cli.API.Responses;
using Refit;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Clients.Cli.API
{
    public interface ICartrackOMDbApi
    {
        #region Titles

        [Get("/api/v1/titles/{imdbId}")]
        Task<GetTitleResponse> GetTitlesByIdAsync(string imdbId);

        [Get("/api/v1/titles/title/{searchTerm}")]
        Task<TitleSearchResponse> SearchTitlesAsync(string searchTerm, [Query] int? year = null);

        #endregion

        #region CachedEntries

        [Get("/api/v1/cachedentries")]
        Task<dynamic> QueryCachedEntriesAsync([Query] ODataQueryParams odataQueryParams = null);

        [Get("/api/v1/cachedentries/{imdbID}")]
        Task<GetTitleResponse> GetCachedEntryAsync(string imdbId);

        [Post("/api/v1/cachedentries")]
        Task<GetTitleResponse> CreateCachedEntryAsync([Body] CreateOrUpdateCacheEntryRequest request);

        [Put("/api/v1/cachedentries/{imdbID}")]
        Task<GetTitleResponse> UpdateCachedEntryAsync(string imdbId, [Body] CreateOrUpdateCacheEntryRequest request);

        [Delete("/api/v1/cachedentries/{imdbID}")]
        Task DeleteCachedEntryAsync(string imdbId);

        #endregion


    }
}