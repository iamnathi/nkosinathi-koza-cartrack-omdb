using Cartrack.OMDb.Web.Models.Requests;
using Cartrack.OMDb.Web.Models.Results;
using Cartrack.OMDb.Web.Models.Results.Models;
using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Application.Services
{
    public interface IMoviesService
    {
        Task<OneOf<GetMovieByIdResult, GetMovieErrorResult>> GetMovieByIdAsync(GetMovieByIdRequest request);
        Task<OneOf<GetMovieByTitleResult, GetMovieErrorResult>> GetMovieByTitleAsync(GetMovieByTitleRequest request);

        IEnumerable<MovieResult> GetCachedEntries();
    }
}