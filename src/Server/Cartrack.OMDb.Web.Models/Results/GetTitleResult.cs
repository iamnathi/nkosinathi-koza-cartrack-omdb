using Cartrack.OMDb.Web.Models.Results.Models;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetTitleResult
    {
        public TitleResult Movie { get; set; }

        public GetTitleResult(TitleResult movie)
        {
            Movie = movie;
        }
    }
}
