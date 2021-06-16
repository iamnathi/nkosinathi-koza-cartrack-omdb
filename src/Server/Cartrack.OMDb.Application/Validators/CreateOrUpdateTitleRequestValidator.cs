using Cartrack.OMDb.Web.Models.Requests;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cartrack.OMDb.Application.Validators
{
    public class CreateOrUpdateTitleRequestValidator : AbstractValidator<CreateOrUpdateTitleRequest>
    {
        protected string[] ValidTypes = new string[] { "movie", "series", "episode" };

        public CreateOrUpdateTitleRequestValidator()
        {
            RuleFor(prop => prop.IMDbID)
                .Matches(@"ev\d{7}\/\d{4}(-\d)?|(ch|co|ev|nm|tt)\d{7}", RegexOptions.IgnoreCase)
                .WithMessage("Please provide a valid IMDb ID.");

            RuleFor(prop => prop.Title)
                .NotEmpty()
                .WithMessage("Please provide a title for the movie, series, or episode.");

            RuleFor(prop => prop.Year)
                .NotEmpty()
                .WithMessage("Please provide the year or period for the movie, series, or episode.");

            RuleFor(prop => prop.Type)
                .Must(type => ValidTypes.Contains(type))
                .WithMessage("Please provide a valid type for the entry. The type value can be either movie, series, or episode.");
        }
    }
}