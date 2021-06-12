namespace Cartrack.OMDb.Web.Models.Omdb
{
    public abstract class ResponseBase
    {
        public string Response { get; set; }
        public string Error { get; set; }

        public bool MovieFound() => Response.Equals("True", System.StringComparison.InvariantCultureIgnoreCase);
    }
}