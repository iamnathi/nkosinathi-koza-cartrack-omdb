using System.Text.Json.Serialization;

namespace Cartrack.OMDb.Clients.Cli.API.Responses
{
    public class TitleResponse
    {
        [JsonPropertyName("imDbID")]
        public string IMDbID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("poster")]
        public string Poster { get; set; }
    }
}
