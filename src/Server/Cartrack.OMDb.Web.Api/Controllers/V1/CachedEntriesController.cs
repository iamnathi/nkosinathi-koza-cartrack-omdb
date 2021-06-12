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
        private readonly ITitleService _titleService;

        public CachedEntriesController(ILoggerFactory loggerFactory, ITitleService moviesService) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CachedEntriesController>();
            _titleService = moviesService ?? throw new ArgumentNullException(nameof(moviesService));
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
                return await Task.FromResult(Ok(_titleService.GetCachedTitles()));
            }, "Get all cached entries");
        }


        [HttpPost]
        public async Task<IActionResult> CreateCacheEntry([FromBody] CreateOrUpdateTitleRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _titleService.SaveOrUpdateTitleAsync(request);
                return response.Match((result) =>
                {
                    return Ok(result.Movie);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Create a cache entry and save to db.");
        }

        [HttpGet]
        [Route("{imdbID}")]
        public async Task<IActionResult> GetCacheEntry([FromRoute] string imdbID)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var movie = _titleService.GetCachedTitles()
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
        public async Task<IActionResult> UpdateCacheEntry([FromRoute] string imdbID, [FromBody] CreateOrUpdateTitleRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _titleService.SaveOrUpdateTitleAsync(request, true);

                return response.Match((result) =>
                {
                    return Ok(result.Movie);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Update a cache entry and save to db.");
        }

        [HttpDelete]
        [Route("{imdbID}")]
        public async Task<IActionResult> DeleteCacheEntry([FromRoute] string imdbID)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var isSuccessful = await _titleService.DeleteTitleAsync(imdbID);
                if (isSuccessful)
                {
                    return Ok();
                }

                return BadRequest();

            }, "Delete cached entries by key");
        }
    }
}
