using CommandLine;

namespace Cartrack.OMDb.Clients.Cli.Options
{
    [Verb("delete", HelpText = "Delete a cache entry for a title.")]
    public class DeleteCacheEntryOptions
    {
        [Option('i', "IMDb ID", Required = true, HelpText = "Provides the IMDb ID")]
        public string IMDbID { get; set; }
    }
}