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

    }
}
