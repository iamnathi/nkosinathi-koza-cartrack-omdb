using System.Collections.Generic;

namespace Cartrack.OMDb.Web.Models.Results
{
    public class ErrorResult
    {
        public int StatusCode { get; set; }
        public IReadOnlyCollection<string> ErrorMessages { get; set; }

        public static ErrorResult FromError(int statusCode, IReadOnlyCollection<string> errorMessages)
        {
            return new ErrorResult
            {
                StatusCode = statusCode,
                ErrorMessages = errorMessages
            };
        }
    }
}