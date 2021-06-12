using Cartrack.OMDb.Web.Models.Requests;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Cartrack.OMDb.Application.Validators
{
    public class GetMovieByIdRequestValidator : AbstractValidator<GetMovieByIdRequest>
    {
        public GetMovieByIdRequestValidator()
        {
            RuleFor(prop => prop.IMDbID)
                .Matches(@"ev\d{7}\/\d{4}(-\d)?|(ch|co|ev|nm|tt)\d{7}", RegexOptions.IgnoreCase)
                .WithMessage("Please provide a valid IMDb ID.");
        }
    }
}