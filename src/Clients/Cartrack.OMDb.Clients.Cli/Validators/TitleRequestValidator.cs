using Cartrack.OMDb.Clients.Cli.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Cartrack.OMDb.Clients.Cli.Validators
{
    public class TitleRequestValidator : AbstractValidator<TitleRequest>
    {
        public TitleRequestValidator()
        {
            RuleFor(prop => prop.IMDbID)
                .Matches(@"ev\d{7}\/\d{4}(-\d)?|(ch|co|ev|nm|tt)\d{7}", RegexOptions.IgnoreCase)
                .WithMessage("Please provide a valid IMDb ID.");
        }
    }
}