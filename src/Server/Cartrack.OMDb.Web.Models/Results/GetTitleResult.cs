using Cartrack.OMDb.Web.Models.Results.Models;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetTitleResult
    {
        public TitleResult Title { get; set; }

        public GetTitleResult(TitleResult movie)
        {
            Title = movie;
        }
    }
}
