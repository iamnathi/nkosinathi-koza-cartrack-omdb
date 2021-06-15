using System.Collections.Generic;

namespace Cartrack.OMDb.Clients.Cli.Models
{
    public class TitleErrorResult
    {
        public IReadOnlyCollection<string> ErrorMessages { get; private set; }

        public TitleErrorResult(IReadOnlyCollection<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
