using System.Text.Json.Serialization;

namespace Cartrack.OMDb.Clients.Cli.API.Responses
{
    public class GetTitleResponse
    {
        [JsonPropertyName("title")]
        public TitleResponse Title { get; set; }
    }
}