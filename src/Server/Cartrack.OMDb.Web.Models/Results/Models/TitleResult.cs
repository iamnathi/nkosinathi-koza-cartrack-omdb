using System.ComponentModel.DataAnnotations;

namespace Cartrack.OMDb.Web.Models.Results.Models
{
    public class TitleResult
    {
        [Key]
        public string IMDbID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
    }
}