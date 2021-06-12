using Cartrack.OMDb.Web.Models.Requests;
using FluentValidation;

namespace Cartrack.OMDb.Application.Validators
{
    public class GetMovieByIdRequestValidator : AbstractValidator<GetMovieByIdRequest>
    {
        public GetMovieByIdRequestValidator()
        {
            RuleFor(prop => prop.IMDbID)
                .NotEmpty()
                .WithMessage("Please provide a valid IMDb ID.");
        }
    }
}