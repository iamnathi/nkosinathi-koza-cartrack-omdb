namespace Cartrack.OMDb.Web.Models.Requests
{
    public class SearchTitlesRequest
    {
        public string SearchTerm { get; set; }
        public int? Year { get; set; }
        public int PageNumber { get; set; }

        public SearchTitlesRequest(string searchTerm, int? year = null, int pageNumber = 1)
        {
            SearchTerm = searchTerm;
            Year = year;
            PageNumber = pageNumber;
        }

        public override string ToString()
        {
            return Year.HasValue
                ? $"s={SearchTerm}&y={Year}"
                : $"s={SearchTerm}";
        }
    }
}