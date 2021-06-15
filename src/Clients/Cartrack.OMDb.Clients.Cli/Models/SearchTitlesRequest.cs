namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class SearchTitlesRequest
    {
        public string SearchTerm { get; private set; }
        public int? Year { get; private set; }

        public SearchTitlesRequest(string searchTerm, int? year = null)
        {
            SearchTerm = searchTerm;
            Year = year;
        }
    }
}