namespace Cartrack.OMDb.Clients.Cli.API.Requests
{
    public class CreateOrUpdateCacheEntryRequest
    {
        public string IMDbID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
    }
}