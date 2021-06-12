using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        public async Task<IActionResult> GetEntriesAsync()
        {
            return await InvokeAppServiceAsync(async () =>
            {
                return await Task.FromResult(Ok(_moviesService.GetCachedEntries()));
            }, "Get all cached entries");
        }


        [HttpPost]
        public async Task<IActionResult> CreateCacheEntry([FromBody] CreateMovieRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {


                return await Task.FromResult(Ok(_moviesService.GetCachedEntries()));
            }, "Create a cache entry and save to db.");
        }

        [HttpGet]
        [Route("{imdbID}")]
        public async Task<IActionResult> GetCacheEntry([FromRoute] string imdbID)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var movie = _moviesService.GetCachedEntries()
                        .SingleOrDefault(c => c.IMDbID.Equals(imdbID, StringComparison.InvariantCultureIgnoreCase));

                if (movie != null)
                {
                    return await Task.FromResult(Ok(movie));
                }

                return await Task.FromResult(NotFound());

            }, "Get cached entries by key");
        }

        [HttpPut]
        [Route("{imdbID}")]
        public async Task<IActionResult> UpdateCacheEntry([FromRoute] string imdbID, [FromBody] CreateMovieRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var movie = _moviesService.GetCachedEntries()
                        .SingleOrDefault(c => c.IMDbID.Equals(imdbID, StringComparison.InvariantCultureIgnoreCase));

                if (movie != null)
                {
                    return await Task.FromResult(Ok(movie));
                }

                return await Task.FromResult(NotFound());

            }, "Get cached entries by key");
        }
    }
}
