using Cartrack.OMDb.Clients.Cli.Models;
using FluentValidation;

namespace Cartrack.OMDb.Clients.Cli.Validators
{
    public class SearchTitlesRequestValidator : AbstractValidator<SearchTitlesRequest>
    {
        public SearchTitlesRequestValidator()
        {
            RuleFor(prop => prop.SearchTerm)
                .NotEmpty()
                .WithMessage("Please provide a title for the movie, series, or episode.");
        }
    }
}