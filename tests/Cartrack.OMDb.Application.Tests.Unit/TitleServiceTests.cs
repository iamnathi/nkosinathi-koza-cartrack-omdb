using AutoFixture;
using AutoFixture.Xunit2;
using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Application.Configurations;
using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Application.Tests.Unit.Http;
using Cartrack.OMDb.Application.Tests.Unit.Http.AutoFixture;
using Cartrack.OMDb.Application.Validators;
using Cartrack.OMDb.Repositories;
using Cartrack.OMDb.Web.Models.Omdb;
using Cartrack.OMDb.Web.Models.Requests;
using Cartrack.OMDb.Web.Models.Results;
using Cartrack.OMDb.Web.Models.Results.Models;
using FluentAssertions;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cartrack.OMDb.Application.Tests.Unit
{
    public class TitleServiceTests : TitleServiceTestsBase
    {
        private TitleService _sut;

        private readonly ILogger<TitleService> _logger = Substitute.For<ILogger<TitleService>>();
        private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
        private HttpClient _apiClient = Substitute.For<HttpClient>();
        private readonly IOptions<OmdbApiSettings> _omdbApiSettings = Substitute.For<IOptions<OmdbApiSettings>>();
        private readonly OmdbApiSettings omdbApiSettings = Substitute.For<OmdbApiSettings>();

        private readonly IValidatorFactory _validatorFactory = Substitute.For<IValidatorFactory>();
        private readonly ITitleRespository _titleRepository = Substitute.For<ITitleRespository>();
        private readonly ICacheProvider<TitleResult> _cacheProvider = Substitute.For<ICacheProvider<TitleResult>>();

        public TitleServiceTests()
        {
            _omdbApiSettings.Value.Returns(new OmdbApiSettings
            {
                ApiKey = "apiKey",
                BaseUrl = BaseAddressUri.ToString()
            });
        }

        [Fact]
        public async Task GetTitleByIdAsync_ShouldReturnResults_WhenIMDbIDIsValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            var request = new GetTitleByIdRequest(imdbID);

            var expectedResult = AutoFixture.Create<GetTitleResponse>();
            expectedResult.Response = "True";

            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResult))
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = BaseAddressUri
            };

            httpClientFactoryMock.CreateClient(StringConstants.OmdbApiClientName).ReturnsForAnyArgs(fakeHttpClient);

            _validatorFactory.GetValidator<GetTitleByIdRequest>().Returns(new GetTitleByIdRequestValidator());


            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, httpClientFactoryMock, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.GetTitleByIdAsync(request);

            // Assert
            result.AsT0.Should().Equals(expectedResult);
        }         

        [Fact]
        public async Task GetTitleByIdAsync_ShouldReturnError_WhenIMDbIDIsInValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            var request = new GetTitleByIdRequest(imdbID);

            var errorMessages = new string[] { "Please provide a valid IMDb ID." };
            var expectedResult = ErrorResult.FromError(400, errorMessages);

            _validatorFactory.GetValidator<GetTitleByIdRequest>().Returns(new GetTitleByIdRequestValidator());



            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, _httpClientFactory, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.GetTitleByIdAsync(request);

            // Assert
            result.AsT1.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task SearchTitlesAsync_ShouldReturnResults_WhenSearchTermIsValid()
        {
            // Arrange
            const string searchTerm = "avangers";
            var request = new SearchTitlesRequest(searchTerm);

            var expectedResult = AutoFixture.Create<SearchMovieResponse>();
            expectedResult.TotalResults = "3";
            expectedResult.Response = "True";

            var expectedResultJSon = JsonSerializer.Serialize(expectedResult);
            

            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResultJSon)
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = BaseAddressUri
            };

            httpClientFactoryMock.CreateClient(StringConstants.OmdbApiClientName).ReturnsForAnyArgs(fakeHttpClient);

            _validatorFactory.GetValidator<SearchTitlesRequest>().Returns(new SearchTitlesRequestValidator());


            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, httpClientFactoryMock, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.SearchTitlesAsync(request);

            // Assert
            result.AsT0.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task SearchTitlesAsync_ShouldReturnError_WhenSearchTermIsInValid()
        {
            // Arrange
            const string searchTerm = "";
            var request = new SearchTitlesRequest(searchTerm);

            var errorMessages = new string[] { "Please provide a title for the movie, series, or episode." };
            var expectedResult = ErrorResult.FromError(400, errorMessages);

            _validatorFactory.GetValidator<SearchTitlesRequest>().Returns(new SearchTitlesRequestValidator());



            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, _httpClientFactory, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.SearchTitlesAsync(request);

            // Assert
            result.AsT1.Should().Equals(expectedResult);
        }


        [Fact]
        public async Task SaveOrUpdateTitleAsync_ShouldReturnTitleAddedToCache_WhenCreateRequestIsValid()
        {
            // Arrange
            const string imdbID = "tt9140554";
            const string title = "The Life story of Nkosinathi (Name) Koza";
            const string year = "2021";
            const string type = "series";

            var request = new CreateOrUpdateTitleRequest
            {
                IMDbID = imdbID,
                Title = title,
                Year = year,
                Type = type
            };

            var expectedResult = new GetTitleResult(request.Adapt<TitleResult>());
            _validatorFactory.GetValidator<CreateOrUpdateTitleRequest>().ReturnsForAnyArgs(new CreateOrUpdateTitleRequestValidator());


            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, _httpClientFactory, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.SaveOrUpdateTitleAsync(request);

            // Assert
            result.AsT0.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task SaveOrUpdateTitleAsync_ShouldReturnError_WhenCreateRequestIsInValid()
        {
            // Arrange
            const string imdbID = "";
            const string title = "";
            const string year = "";
            const string type = "";

            var request = new CreateOrUpdateTitleRequest
            {
                IMDbID = imdbID,
                Title = title,
                Year = year,
                Type = type
            };
            var errorMessages = new string[] 
            {
                "Please provide a valid IMDb ID.",
                "Please provide a title for the movie, series, or episode.",
                "Please provide the year or period for the movie, series, or episode.",
                "Please provide a valid type for the entry. The type value can be either movie, series, or episode."
            };
            var expectedResult = ErrorResult.FromError(400, errorMessages);


            _validatorFactory.GetValidator<CreateOrUpdateTitleRequest>().ReturnsForAnyArgs(new CreateOrUpdateTitleRequestValidator());


            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, _httpClientFactory, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.SaveOrUpdateTitleAsync(request);

            // Assert
            result.AsT1.Should().Equals(expectedResult);
        }

        [Fact]
        public async Task DeleteTitleAsync_ShouldReturnTrue_WhenIMDbIDIsValid()
        {
            // Arrange
            const string imdbID = "tt9140554";

            // Act
            _sut = new TitleService(_logger, _omdbApiSettings, _httpClientFactory, _validatorFactory, _titleRepository, _cacheProvider);
            var result = await _sut.DeleteTitleAsync(imdbID);

            // Assert
            result.Should().BeTrue();
        }
    }
}
