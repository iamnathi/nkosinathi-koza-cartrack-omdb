using AutoFixture;
using Cartrack.OMDb.Clients.Cli.API;
using Cartrack.OMDb.Clients.Cli.API.Requests;
using Cartrack.OMDb.Clients.Cli.API.Responses;
using Cartrack.OMDb.Clients.Cli.Models;
using Cartrack.OMDb.Clients.Cli.Services;
using Cartrack.OMDb.Clients.Cli.Validators;
using FluentAssertions;
using FluentValidation;
using Mapster;
using NSubstitute;
using System.Collections.Generic;
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
            // Arrange
            const string searchTerm = "avengers";
            var request = new SearchTitlesRequest(searchTerm);
            var apiResesponse = _fixture.Create<TitleSearchResponse>();
            _titleApi.SearchTitlesAsync(searchTerm).ReturnsForAnyArgs(apiResesponse);
            var expectedResult = new SearchTitlesResult
            {
                Titles = apiResesponse.Titles.Adapt<IReadOnlyCollection<TitleResult>>(),
                PageNumber = apiResesponse.PageNumber,
                PageSize = apiResesponse.PageSize,
                TotalTitlesCount = apiResesponse.TotalTitlesCount
            };

            // Act
            var result = await _sut.SearchTitlesAsync(request);

            // Assert
            result.AsT0.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task SearchTitlesAsync_ShouldReturnError_WhenSearchTermIsInValid()
        {
            // Arrange
            const string searchTerm = "";
            var request = new SearchTitlesRequest(searchTerm);
            var errorMessages = new string[] { "Please provide a term to search for movies, series, or episodes with." };
            var expectedResult = new TitleErrorResult(errorMessages);

            // Act
            var result = await _sut.SearchTitlesAsync(request);

            // Assert
            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CreateCacheEntryAsync_ShouldReturnResults_WhenCreateRequestIsValid()
        {
            // Arange
            const string imdbID = "tt9140554";
            const string title = "Loki";
            const string year = "2021–";
            const string type = "series";

            var request = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            var apiRequest = new CreateOrUpdateCacheEntryRequest()
            {
                IMDbID = imdbID,
                Title = title,
                Year = year,
                Type = type
            };
            var apiResponse = _fixture.Create<GetTitleResponse>();
            _titleApi.CreateCachedEntryAsync(apiRequest).ReturnsForAnyArgs(apiResponse);
            var expectedResult = new CreateOrUpdateTitleResult
            {
                Title = new TitleResult
                {
                    IMDbID = apiResponse.Title.IMDbID,
                    Title = apiResponse.Title.Title,
                    Year = apiResponse.Title.Year,
                    Type = apiResponse.Title.Type,
                }
            };


            // Act
            var result = await _sut.CreateCacheEntryAsync(request);


            // Assert
            result.AsT0.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CreateCacheEntryAsync_ShouldReturnError_WhenCreateRequestIsInValid()
        {
            // Arange
            const string imdbID = "tt9140554";
            const string title = "";
            const string year = "2021–";
            const string type = "series";

            var request = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            var errorMessages = new string[] { "Please provide a title for the movie, series, or episode." };
            var expectedResult = new TitleErrorResult(errorMessages);


            // Act
            var result = await _sut.CreateCacheEntryAsync(request);


            // Assert
            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task UpdateCacheEntryAsync_ShouldReturnResults_WhenUpdateRequestIsValid()
        {
            // Arange
            const string imdbID = "tt9140554";
            const string title = "Loki from the past";
            const string year = "2017–2021";
            const string type = "series";

            var request = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            var apiRequest = new CreateOrUpdateCacheEntryRequest()
            {
                IMDbID = imdbID,
                Title = title,
                Year = year,
                Type = type
            };
            var apiResponse = _fixture.Create<GetTitleResponse>();
            _titleApi.UpdateCachedEntryAsync(imdbID, apiRequest).ReturnsForAnyArgs(apiResponse);
            var expectedResult = new CreateOrUpdateTitleResult
            {
                Title = new TitleResult
                {
                    IMDbID = apiResponse.Title.IMDbID,
                    Title = apiResponse.Title.Title,
                    Year = apiResponse.Title.Year,
                    Type = apiResponse.Title.Type,
                }
            };


            // Act
            var result = await _sut.UpdateCacheEntryAsync(request);


            // Assert
            result.AsT0.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task UpdateCacheEntryAsync_ShouldReturnError_WhenUpdateRequestIsInValid()
        {
            // Arange
            const string imdbID = "tt9140554";
            const string title = "";
            const string year = "";
            const string type = "";

            var request = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            var errorMessages = new string[] 
            { 
                "Please provide a title for the movie, series, or episode.",
                "Please provide the year or period for the movie, series, or episode.",
                "Please provide a valid type for the entry. The type value can be either movie, series, or episode."
            };
            var expectedResult = new TitleErrorResult(errorMessages);


            // Act
            var result = await _sut.UpdateCacheEntryAsync(request);


            // Assert
            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task DeleteCacheEntryAsync_ShouldReturnTrue_WhenIMDbIDIsValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            var request = new TitleRequest(imdbID);
            var apiResesponse = _fixture.Create<Task>();
            _titleApi.DeleteCachedEntryAsync(imdbID).ReturnsForAnyArgs(apiResesponse);
            var expectedResult = true;

            // Act
            var result = await _sut.DeleteCacheEntryAsync(request);

            // Assert
            result.AsT0.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task DeleteCacheEntryAsync_ShouldReturnError_WhenIMDbIDIsInValid()
        {
            // Arrange
            const string imdbID = "cd26ccf6-75d6-4521-884f-1693c62ed303";
            var request = new TitleRequest(imdbID);
            var errorMessages = new string[] { "Please provide a valid IMDb ID." };
            var expectedResult = new TitleErrorResult(errorMessages);


            // Act
            var result = await _sut.DeleteCacheEntryAsync(request);

            // Assert
            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }
    }
}