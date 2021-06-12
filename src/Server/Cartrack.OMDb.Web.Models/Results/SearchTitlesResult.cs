using Cartrack.OMDb.Web.Models.Results.Models;
using System.Collections.Generic;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class SearchTitlesResult
    {
        public IEnumerable<TitleResult> Movies { get; set; }

        public SearchTitlesResult(IEnumerable<TitleResult> movies)
        {
            Movies = movies;
        }
    }
}