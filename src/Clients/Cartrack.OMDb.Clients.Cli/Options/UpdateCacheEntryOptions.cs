using CommandLine;

namespace Cartrack.OMDb.Clients.Cli.Options
{
    [Verb("update", HelpText = "Update cache entry for a title.")]
    public class UpdateCacheEntryOptions
    {
        [Option('i', "IMDb ID", Required = true, HelpText = "Provides the IMDb ID.")]
        public string IMDbID { get; private set; }

        [Option('n', "Name", Required = true, HelpText = "Provides the name of the title.")]
        public string Name { get; private set; }

        [Option('y', "Year", Required = true, HelpText = "Provides the year the of release.")]
        public string Year { get; private set; }

        [Option('t', "Type", Required = true, HelpText = "Provides the type of the title which is either a movie, series, or episode.")]
        public string Type { get; private set; }
    }
}