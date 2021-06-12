using Cartrack.OMDb.Web.Models.Requests;
using FluentValidation;

namespace Cartrack.OMDb.Application.Validators
{
    public class GetMovieByTitleRequestValidator : AbstractValidator<GetMovieByTitleRequest>
    {
        public GetMovieByTitleRequestValidator()
        {
            RuleFor(prop => prop.Title)
                .NotEmpty()
                .WithMessage("Please provide a movie title.");


        }
    }
}