using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Application.Configurations;
using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Repositories;
using Cartrack.OMDb.Web.Models.Results.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;

namespace Cartrack.OMDb.Application.Tests.Unit
{
    public class TitleServiceTests
    {
        private readonly TitleService _sut;

        private readonly ILogger<TitleService> _logger = Substitute.For<ILogger<TitleService>>();
        private readonly HttpClient _httpClient = Substitute.For<HttpClient>();
        private readonly OmdbApiSettings _omdbApiSettings = Substitute.For<OmdbApiSettings>();

        private readonly IValidatorFactory _validatorFactory = Substitute.For<IValidatorFactory>();
        private readonly ITitleRespository _titleRepository = Substitute.For<ITitleRespository>();
        private readonly ICacheProvider<TitleResult> _cacheProvider = Substitute.For<ICacheProvider<TitleResult>>();

        public TitleServiceTests()
        {

        }


    }
}
