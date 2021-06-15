namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class TitleRequest
    {
        public string IMDbID { get; private set; }

        public TitleRequest(string imbID)
        {
            IMDbID = imbID;
        }
    }
}
