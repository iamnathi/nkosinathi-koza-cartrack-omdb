using Cartrack.OMDb.Web.Models.Requests;
using Cartrack.OMDb.Web.Models.Results;
using Cartrack.OMDb.Web.Models.Results.Models;
using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Application.Services
{
    public interface ITitleService
    {
        Task<OneOf<GetTitleResult, ErrorResult>> GetTitleByIdAsync(GetTitleByIdRequest request);
        Task<OneOf<SearchTitlesResult, ErrorResult>> SearchTitlesAsync(SearchTitlesRequest request);
        Task<OneOf<GetTitleResult, ErrorResult>> SaveOrUpdateTitleAsync(CreateOrUpdateTitleRequest request, bool isUpdate = false);

        IEnumerable<TitleResult> GetCachedTitles();        
        Task<bool> DeleteTitleAsync(string imdbId);
    }
}