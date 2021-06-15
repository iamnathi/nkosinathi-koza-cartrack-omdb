using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Web.Models.Requests;
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
        private readonly ITitleService _titleService;

        public CachedEntriesController(ILoggerFactory loggerFactory, ITitleService moviesService) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CachedEntriesController>();
            _titleService = moviesService ?? throw new ArgumentNullException(nameof(moviesService));
        }

        /// <summary>
        /// Get all cached entries (OData enabled endpoint)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetEntriesAsync()
        {
            return await InvokeAppServiceAsync(async () =>
            {
                return await Task.FromResult(Ok(_titleService.GetCachedTitles()));
            }, "Get all cached entries");
        }


        /// <summary>
        /// Create a cache entry.
        /// </summary>
        /// <param name="request">Request object with mandatory fields for creating a title entry</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCacheEntry([FromBody] CreateOrUpdateTitleRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _titleService.SaveOrUpdateTitleAsync(request);
                return response.Match((result) =>
                {
                    return Ok(result);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Create a cache entry.");
        }

        /// <summary>
        /// Get cached entries by IMDbID
        /// </summary>
        /// <param name="imdbID">The IMDbID of the cached entry to update</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{imdbID}")]
        public async Task<IActionResult> UpdateCacheEntry([FromRoute] string imdbID, [FromBody] CreateOrUpdateTitleRequest request)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _titleService.SaveOrUpdateTitleAsync(request, true);

                return response.Match((result) =>
                {
                    return Ok(result);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Update a cache entry and save to db.");
        }

        /// <summary>
        /// Delete cache entry by IMDbID
        /// </summary>
        /// <param name="imdbID">The IMDbID of the cached entry to delete</param>
        /// <returns></returns>
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
