using Cartrack.OMDb.Web.Models.Results.Models;
using System.Collections.Generic;
using System.Linq;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class SearchTitlesResult
    {
        public IEnumerable<TitleResult> Titles { get; set; }

        public int TotalTitlesCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize => Titles?.Count() ?? 0;

        public SearchTitlesResult(IEnumerable<TitleResult> movies, int totalTitlesCount, int pageNumber)
        {
            Titles = movies;
            TotalTitlesCount = totalTitlesCount;
            PageNumber = pageNumber;
        }
    }
}