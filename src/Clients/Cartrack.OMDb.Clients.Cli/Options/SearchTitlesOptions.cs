using CommandLine;

namespace Cartrack.OMDb.Clients.Cli.Options
{
    [Verb("search", HelpText = "Search titles on OMDb API.")]
    public class SearchTitlesOptions
    {
        [Option('s', "Search", Required = true, HelpText = "Provides the search term to perform the search on")]
        public string SearchTerm { get; set; }

        [Option('y', "Year", HelpText = "Provides the year to filter the search on")]
        public int? Year { get; set; }
    }
}