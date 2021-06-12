namespace Cartrack.OMDb.Web.Models.Requests
{
    public class GetTitleByIdRequest
    {
        public string IMDbID { get; set; }

        public GetTitleByIdRequest(string imdbID)
        {
            IMDbID = imdbID;
        }

        public override string ToString()
        {
            return $"i={IMDbID}";
        }
    }
}