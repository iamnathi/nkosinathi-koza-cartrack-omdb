using Refit;

namespace Cartrack.OMDb.Clients.Cli.API.Requests
{
    public class ODataQueryParams
    {
        [AliasAs("$select")]
        public string Select { get; set; }

        [AliasAs("$filter")]
        public string Where { get; set; }

        [AliasAs("$orderby")]
        public string OrderBy { get; set; }

        [AliasAs("$count")]
        public string Count { get; set; }

        public ODataQueryParams(string select = null, string where = null, string orderBy = null, string count = null)
        {
            Select = select;
            Where = where;
            OrderBy = orderBy;
            Count = count;
        }
    }
}