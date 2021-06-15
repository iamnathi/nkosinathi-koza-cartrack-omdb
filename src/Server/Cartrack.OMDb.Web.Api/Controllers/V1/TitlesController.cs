using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class TitlesController : ApiControllerBase
    {
        private readonly ILogger<TitlesController> _logger;
        private readonly ITitleService _moviesService;

        public TitlesController(ILoggerFactory loggerFactory, ITitleService moviesService) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TitlesController>();
            _moviesService = moviesService ?? throw new ArgumentNullException(nameof(moviesService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imdbId">IMDb ID (e.g </param>
        /// <returns></returns>
        [HttpGet]
        [Route("{imdbId}")]
        public async Task<IActionResult> GetTitleByIdAsync([FromRoute] string imdbId)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _moviesService.GetTitleByIdAsync(new GetTitleByIdRequest(imdbId));

                return response.Match((result) =>
                {
                    return Ok(result);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Get movie by IMDb ID.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm">Search term to search titles for.</param>
        /// <param name="year">Year of release.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("title/{searchTerm}")]
        public async Task<IActionResult> SearchTitlesAsync([FromRoute] string searchTerm, [FromQuery] int? year)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _moviesService.SearchTitlesAsync(new SearchTitlesRequest(searchTerm, year));

                return response.Match((result) =>
                {
                    return Ok(result);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Search titles using search term and/or year.");
        }

    }
}
