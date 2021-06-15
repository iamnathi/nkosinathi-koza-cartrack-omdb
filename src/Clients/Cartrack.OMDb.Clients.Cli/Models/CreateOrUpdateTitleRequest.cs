namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class CreateOrUpdateTitleRequest
    {
        public string IMDbID { get; private set; }
        public string Title { get; private set; }
        public string Year { get; private set; }
        public string Type { get; private set; }

        public CreateOrUpdateTitleRequest(string imdbID, string title, string year, string type)
        {
            IMDbID = imdbID;
            Title = title;
            Year = year;
            Type = type;
        }
    }
}