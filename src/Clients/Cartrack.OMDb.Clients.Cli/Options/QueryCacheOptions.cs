using CommandLine;

namespace Cartrack.OMDb.Clients.Cli.Options
{
    [Verb("query", HelpText = "Query the cached title entries using OData.")]
    public class QueryCacheOptions
    {
        [Option('s', "SELECT", HelpText = "OData $select query to specify properties to return.")]
        public string Select { get; set; }

        [Option('w', "WHERE", HelpText = "OData $filter query to filter the collection of cached title entries.")]
        public string Where { get; set; }

        [Option('o', "ORDERBY", HelpText = "OData $orderby query to sort the collection of cached title entries.")]
        public string OrderBy { get; set; }

        [Option('c', "COUNT", HelpText = "OData $count aggregate query to count the number of cached entries.")]
        public string Count { get; set; }
    }
}