using AutoFixture;
using Cartrack.OMDb.Clients.Cli.Extensions;
using Cartrack.OMDb.Clients.Cli.Models;
using Cartrack.OMDb.Clients.Cli.Services;
using NSubstitute;
using OneOf;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cartrack.OMDb.Clients.Cli.Tests.Unit
{
    public class CartrackOMDbApplicationTests
    {
        private readonly CartrackOMDbApplication _sut;

        private readonly IConsoleWriter _consoleWriter = Substitute.For<IConsoleWriter>();
        private readonly ITitlesService _titleService = Substitute.For<ITitlesService>();
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly IFixture _fixture = new Fixture();

        public CartrackOMDbApplicationTests()
        {
            _sut = new CartrackOMDbApplication(_consoleWriter, _titleService);
        }

        [Fact]
        public async Task RunAsync_ShouldReturnATitle_WhenIMDbIdIsValid()
        {
            // Arrange
            const string imdbId = "tt9140554";
            var args = new string[] { "get", "-i", imdbId }.ParseWithSpace();
            
            var titleResult = _fixture.Create<GetTitleResult>();
            OneOf<GetTitleResult, TitleErrorResult> result = titleResult;

            var getRequest = new TitleRequest(imdbId);
            _titleService.GetTitleByIMDbIdAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = JsonSerializer.Serialize(titleResult, _serializerOptions);
            var consoleForegroundColor = ConsoleColor.Green;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnError_WhenIMDbIdIsInValid()
        {
            // Arrange
            const string imdbId = "0f8fad5b-d9cb-469f-a165-70867728950e";
            var args = new string[] { "get", "-i", imdbId }.ParseWithSpace();

            var errorMessages = new string[] { "Please provide a valid IMDb ID." };
            var expectedResult = new TitleErrorResult(errorMessages);
            OneOf<GetTitleResult, TitleErrorResult> result = expectedResult;

            var getRequest = new TitleRequest(imdbId);
            _titleService.GetTitleByIMDbIdAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = string.Join(Environment.NewLine, expectedResult.ErrorMessages);
            var consoleForegroundColor = ConsoleColor.Red;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnATitle_WhenSearchTermIsValid()
        {
            // Arrange
            const string searchTerm = "star wars";
            var args = new string[] { "search", "-s", searchTerm }.ParseWithSpace();

            var searchResult = _fixture.Create<SearchTitlesResult>();
            OneOf<SearchTitlesResult, TitleErrorResult> result = searchResult;

            var getRequest = new SearchTitlesRequest(searchTerm);
            _titleService.SearchTitlesAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = JsonSerializer.Serialize(searchResult, _serializerOptions);
            var consoleForegroundColor = ConsoleColor.Green;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnError_WhenSearchTermIsInValid()
        {
            // Arrange
            const string searchTerm = "";
            var args = new string[] { "search", "-s", searchTerm }.ParseWithSpace();

            var errorMessages = new string[] { "Please provide a term to search for movies, series, or episodes with." };
            var expectedResult = new TitleErrorResult(errorMessages);
            OneOf<SearchTitlesResult, TitleErrorResult> result = expectedResult;

            var getRequest = new SearchTitlesRequest(searchTerm);
            _titleService.SearchTitlesAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = string.Join(Environment.NewLine, expectedResult.ErrorMessages);
            var consoleForegroundColor = ConsoleColor.Red;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnError_WhenCreateOptionsAreValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            const string title = "Loki";
            const string year = "2017 - 2021";
            const string type = "series";

            var args = new string[] { "create", "-i", imdbID, "-n", title, "-y", year, "-t", type }.ParseWithSpace();

            var searchResult = _fixture.Create<CreateOrUpdateTitleResult>();
            OneOf<CreateOrUpdateTitleResult, TitleErrorResult> result = searchResult;

            var getRequest = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            _titleService.CreateCacheEntryAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = JsonSerializer.Serialize(searchResult, _serializerOptions);
            var consoleForegroundColor = ConsoleColor.Green;

            const string infoText = "Title added to cache.";

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(infoText));
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnResult_WhenCreateOptionsAreInValid()
        {
            // Arrange
            const string imdbID = "cd26ccf6-75d6-4521-884f-1693c62ed303";
            const string title = "";
            const string year = "";
            const string type = "";

            var args = new string[] { "create", "-i", imdbID, "-n", title, "-y", year, "-t", type }.ParseWithSpace();

            var errorMessages = new string[]
            {
                "Please provide a valid IMDb ID.",
                "Please provide a title for the movie, series, or episode.",
                "Please provide the year or period for the movie, series, or episode.",
                "Please provide a valid type for the entry. The type value can be either movie, series, or episode."
            };
            var expectedResult = new TitleErrorResult(errorMessages);
            OneOf<CreateOrUpdateTitleResult, TitleErrorResult> result = expectedResult;

            var createRequest = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            _titleService.CreateCacheEntryAsync(createRequest).ReturnsForAnyArgs(result);
            var expectedResultText = string.Join(Environment.NewLine, expectedResult.ErrorMessages);
            var consoleForegroundColor = ConsoleColor.Red;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnError_WhenUpdateOptionsAreValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            const string title = "Loki";
            const string year = "2017 - 2021";
            const string type = "series";

            var args = new string[] { "update", "-i", imdbID, "-n", title, "-y", year, "-t", type }.ParseWithSpace();

            var searchResult = _fixture.Create<CreateOrUpdateTitleResult>();
            OneOf<CreateOrUpdateTitleResult, TitleErrorResult> result = searchResult;

            var getRequest = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            _titleService.UpdateCacheEntryAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = JsonSerializer.Serialize(searchResult, _serializerOptions);
            var consoleForegroundColor = ConsoleColor.Green;

            const string infoText = "Title cache entry updated.";

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(infoText));
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnResult_WhenUpdateOptionsAreInValid()
        {
            // Arrange
            const string imdbID = "cd26ccf6-75d6-4521-884f-1693c62ed303";
            const string title = "";
            const string year = "";
            const string type = "";

            var args = new string[] { "update", "-i", imdbID, "-n", title, "-y", year, "-t", type }.ParseWithSpace();

            var errorMessages = new string[]
            {
                "Please provide a valid IMDb ID.",
                "Please provide a title for the movie, series, or episode.",
                "Please provide the year or period for the movie, series, or episode.",
                "Please provide a valid type for the entry. The type value can be either movie, series, or episode."
            };
            var expectedResult = new TitleErrorResult(errorMessages);
            OneOf<CreateOrUpdateTitleResult, TitleErrorResult> result = expectedResult;

            var createRequest = new CreateOrUpdateTitleRequest(imdbID, title, year, type);
            _titleService.UpdateCacheEntryAsync(createRequest).ReturnsForAnyArgs(result);
            var expectedResultText = string.Join(Environment.NewLine, expectedResult.ErrorMessages);
            var consoleForegroundColor = ConsoleColor.Red;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnError_WhenDeleteOptionsAreValid()
        {
            // Arrange
            const string imdbId = "tt9140554";
            var args = new string[] { "delete", "-i", imdbId }.ParseWithSpace();

            var titleResult = true;
            OneOf<bool, TitleErrorResult> result = titleResult;

            var getRequest = new TitleRequest(imdbId);
            _titleService.DeleteCacheEntryAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = $"Cache entry {imdbId} deleted.";
            var consoleForegroundColor = ConsoleColor.Yellow;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

        [Fact]
        public async Task RunAsync_ShouldReturnResult_WhenDeleteOptionsAreInValid()
        {
            const string imdbId = "tt9140554";
            var args = new string[] { "delete", "-i", imdbId }.ParseWithSpace();

            var titleResult = false;
            OneOf<bool, TitleErrorResult> result = titleResult;

            var getRequest = new TitleRequest(imdbId);
            _titleService.DeleteCacheEntryAsync(getRequest).ReturnsForAnyArgs(result);
            var expectedResultText = $"Failed to delete cache entry {imdbId}. Please try again later.";
            var consoleForegroundColor = ConsoleColor.Red;

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedResultText), Arg.Is(consoleForegroundColor));
        }

    }
}