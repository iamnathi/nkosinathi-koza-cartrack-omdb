using Cartrack.OMDb.Web.Models.Requests;
using FluentValidation;

namespace Cartrack.OMDb.Application.Validators
{
    public class SearchTitlesRequestValidator : AbstractValidator<SearchTitlesRequest>
    {
        public SearchTitlesRequestValidator()
        {
            RuleFor(prop => prop.Title)
                .NotEmpty()
                .WithMessage("Please provide a title for the movie, series, or episode.");
        }
    }
}