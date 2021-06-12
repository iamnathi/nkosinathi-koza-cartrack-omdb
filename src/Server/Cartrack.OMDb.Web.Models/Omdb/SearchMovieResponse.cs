using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cartrack.OMDb.Web.Models.Omdb
{
    public class SearchMovieResponse : ResponseBase
    {
        [JsonPropertyName("Search")]
        public IEnumerable<GetMovieResponse> Search { get; set; }

        [JsonPropertyName("totalResults")]
        public string TotalResults { get; set; }

        public int TotalSearchCount => int.Parse(TotalResults);
    }
}