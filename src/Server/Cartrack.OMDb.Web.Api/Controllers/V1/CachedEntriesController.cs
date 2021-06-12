using Cartrack.OMDb.Application.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class CachedEntriesController : ApiControllerBase
    {
        private readonly ILogger<CachedEntriesController> _logger;
        private readonly IMoviesService _moviesService;

        public CachedEntriesController(ILoggerFactory loggerFactory, IMoviesService moviesService) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CachedEntriesController>();
            _moviesService = moviesService ?? throw new ArgumentNullException(nameof(moviesService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetEntriesAsync()
        {
            return await InvokeAppServiceAsync(async () =>
            {
                return await Task.FromResult(Ok(_moviesService.GetCachedEntries()));
            }, "Get all cached entries");
        }



    }
}
