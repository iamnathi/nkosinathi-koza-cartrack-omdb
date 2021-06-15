using AutoFixture;
using Cartrack.OMDb.Clients.Cli.API;
using Cartrack.OMDb.Clients.Cli.API.Responses;
using Cartrack.OMDb.Clients.Cli.Models;
using Cartrack.OMDb.Clients.Cli.Services;
using Cartrack.OMDb.Clients.Cli.Validators;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Cartrack.OMDb.Clients.Cli.Tests.Unit
{
    public class TitlesServiceTests
    {
        private readonly TitlesService _sut;

        private readonly ICartrackOMDbApi _titleApi = Substitute.For<ICartrackOMDbApi>();
        private readonly IValidator<TitleRequest> _getTitleValidator = new TitleRequestValidator();
        private readonly IValidator<SearchTitlesRequest> _searchTitleValidator = new SearchTitlesRequestValidator();
        private readonly IValidator<CreateOrUpdateTitleRequest> _createOrUpdateTitleValidator = new CreateOrUpdateTitleRequestValidator();

        private readonly IFixture _fixture = new Fixture();

        public TitlesServiceTests()
        {
            _sut = new TitlesService(_titleApi, _getTitleValidator, _searchTitleValidator, _createOrUpdateTitleValidator);
        }

        [Fact]
        public async Task GetTitleByIMDbIdAsync_ShouldReturnResults_WhenIMDbIDIsValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            var request = new TitleRequest(imdbID);
            var apiResesponse = _fixture.Create<GetTitleResponse>();
            _titleApi.GetTitlesByIdAsync(imdbID).Returns(apiResesponse);
            var expectedResult = new GetTitleResult
            {
                Title = new TitleResult
                {
                    IMDbID = apiResesponse.Title.IMDbID,
                    Title = apiResesponse.Title.Title,
                    Year = apiResesponse.Title.Year,
                    Type = apiResesponse.Title.Type
                }
            };

            // Act
            var result = await _sut.GetTitleByIMDbIdAsync(request);

            // Assert
            result.AsT0.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetTitleByIMDbIdAsync_ShouldReturnError_WhenIMDbIDIsInValid()
        {
            // Arrange
            const string imdbID = "cd26ccf6-75d6-4521-884f-1693c62ed303";
            var request = new TitleRequest(imdbID);
            var errorMessages = new string[] { "Please provide a valid IMDb ID." };
            var expectedResult = new TitleErrorResult(errorMessages);


            // Act
            var result = await _sut.GetTitleByIMDbIdAsync(request);

            // Assert
            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task SearchTitlesAsync_ShouldReturnResults_WhenSearchTermMatchesTitles()
        {

        }
    }
}