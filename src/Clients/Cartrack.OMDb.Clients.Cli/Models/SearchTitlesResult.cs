using System.Collections.Generic;

namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class SearchTitlesResult
    {
        public IReadOnlyCollection<TitleResult> Titles { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalTitlesCount { get; set; }
    }
}