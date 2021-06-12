using System.Collections.Generic;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class GetMovieErrorResult
    {
        public int StatusCode { get; set; }
        public IReadOnlyCollection<string> ErrorMessages { get; set; }

        public static GetMovieErrorResult FromError(int statusCode, IReadOnlyCollection<string> errorMessages)
        {
            return new GetMovieErrorResult
            {
                StatusCode = statusCode,
                ErrorMessages = errorMessages
            };
        }
    }
}