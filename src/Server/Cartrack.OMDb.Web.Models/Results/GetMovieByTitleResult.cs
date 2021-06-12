using Cartrack.OMDb.Web.Models.Results.Models;
using System.Collections.Generic;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetMovieByTitleResult
    {
        public IEnumerable<MovieResult> Movies { get; set; }

        public GetMovieByTitleResult(IEnumerable<MovieResult> movies)
        {
            Movies = movies;
        }
    }
}