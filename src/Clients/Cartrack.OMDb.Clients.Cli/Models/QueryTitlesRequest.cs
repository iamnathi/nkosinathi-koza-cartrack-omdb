namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class QueryTitlesRequest
    {
        public string Select { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public string Count { get; set; }

        public QueryTitlesRequest(string select = null, string where = null, string orderBy = null, string count = null)
        {
            Select = select;
            Where = where;
            OrderBy = orderBy;
            Count = count;
        }
    }
}