namespace Cartrack.OMDb.Web.Models.Requests
{
    public class GetMovieByIdRequest
    {
        public string IMDbID { get; set; }

        public GetMovieByIdRequest(string imdbID)
        {
            IMDbID = imdbID;
        }

        public override string ToString()
        {
            return $"i={IMDbID}";
        }
    }
}