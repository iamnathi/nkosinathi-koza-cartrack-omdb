using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cartrack.OMDb.Web.Models.Omdb
{
    public class GetTitleResponse : ResponseBase
    {
        [JsonPropertyName("imdbID")]
        public string IMDbID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public string Metascore { get; set; }

        [JsonPropertyName("imdbRating")]
        public string IMDbRating { get; set; }

        [JsonPropertyName("imdbVotes")]
        public string IMDbVotes { get; set; }        

        [JsonPropertyName("totalSeasons")]
        public string TotalSeasons { get; set; }

        public IEnumerable<RatingResponse> Ratings { get; set; }
    }
}