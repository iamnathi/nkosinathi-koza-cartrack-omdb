using Cartrack.OMDb.Clients.Cli.Models;
using OneOf;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Clients.Cli.Services
{
    public interface ITitlesService
    {
        Task<OneOf<GetTitleResult, TitleErrorResult>> GetTitleByIMDbIdAsync(TitleRequest request);
        Task<OneOf<SearchTitlesResult, TitleErrorResult>> SearchTitlesAsync(SearchTitlesRequest request);


        Task<OneOf<dynamic, TitleErrorResult>> QueryCacheEntryAsync(QueryTitlesRequest request);
        Task<OneOf<CreateOrUpdateTitleResult, TitleErrorResult>> CreateCacheEntryAsync(CreateOrUpdateTitleRequest request);
        Task<OneOf<CreateOrUpdateTitleResult, TitleErrorResult>> UpdateCacheEntryAsync(CreateOrUpdateTitleRequest request);
        Task<OneOf<bool, TitleErrorResult>> DeleteCacheEntryAsync(TitleRequest request);
    }
}