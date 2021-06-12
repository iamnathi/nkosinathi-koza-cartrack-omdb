namespace Cartrack.OMDb.Web.Models.Requests
{
    public class CreateMovieRequest
    {
        public string IMDbID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}