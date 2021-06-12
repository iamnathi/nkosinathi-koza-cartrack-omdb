using Cartrack.OMDb.Web.Models.Results.Models;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetMovieByIdResult
    {
        public MovieResult Movie { get; set; }

        public GetMovieByIdResult(MovieResult movie)
        {
            Movie = movie;
        }
    }
}
