using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cartrack.OMDb.Clients.Cli.API.Responses
{
    public class TitleSearchResponse
    {
        [JsonPropertyName("titles")]
        public IReadOnlyCollection<TitleResponse> Titles { get; set; }

        [JsonPropertyName("totalTitlesCount")]
        public int TotalTitlesCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
}