using Cartrack.OMDb.Web.Models.Results.Models;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetMovieByTitleResult
    {
        public MovieResult Movie { get; set; }

        public GetMovieByTitleResult(MovieResult movie)
        {
            Movie = movie;
        }
    }
}