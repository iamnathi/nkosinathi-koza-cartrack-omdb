using AutoFixture;
using Cartrack.OMDb.Clients.Cli.Models;
using Cartrack.OMDb.Clients.Cli.Services;
using NSubstitute;
using OneOf;
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
        public async Task RunAsync_ShouldReturnATitle_WhenGetVerbIsParsed()
        {
            // Arrange
            const string imdbID = "tt9140554";
            var args = new string[] { "get", "-i", imdbID };

            var titleResult = _fixture.Create<GetTitleResult>();
            OneOf<GetTitleResult, TitleErrorResult> result = titleResult;

            var request = new TitleRequest(imdbID);
            _titleService.GetTitleByIMDbIdAsync(request).Returns(result);
            var expectedformattedJsonResult = JsonSerializer.Serialize(titleResult, _serializerOptions);

            // Act
            await _sut.RunAsync(args);

            // Assert
            _consoleWriter.Received(1).WriteLine(Arg.Is(expectedformattedJsonResult));
        }
    }
}