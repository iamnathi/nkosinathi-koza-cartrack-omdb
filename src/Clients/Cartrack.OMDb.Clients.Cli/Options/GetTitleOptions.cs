using CommandLine;

namespace Cartrack.OMDb.Clients.Cli.Options
{
    [Verb("get", HelpText = "Get title on OMDb API.")]
    public class GetTitleOptions
    {
        [Option('i', "IMDb ID", Required = true, HelpText = "Provides the IMDb ID")]
        public string IMDbID { get; set; }
    }
}