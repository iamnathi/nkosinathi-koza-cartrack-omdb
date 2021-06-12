using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api.Controllers.V1
{
    [ApiVersion("1")]
    public class MoviesController : ApiControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMoviesService _moviesService;

        public MoviesController(ILoggerFactory loggerFactory, IMoviesService moviesService) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MoviesController>();
            _moviesService = moviesService ?? throw new ArgumentNullException(nameof(moviesService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imdbId">IMDb ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{imdbId}")]
        public async Task<IActionResult> GetMovieByIdAsync([FromRoute] string imdbId)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _moviesService.GetMovieByIdAsync(new GetMovieByIdRequest(imdbId));

                return response.Match((result) =>
                {
                    return Ok(result.Movie);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Get movie by IMDb ID.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">Movie title to search for.</param>
        /// <param name="year">Year of release.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("title/{title}")]
        public async Task<IActionResult> GetMovieByTitleAsync([FromRoute] string title, [FromQuery] int? year)
        {
            return await InvokeAppServiceAsync(async () =>
            {
                var response = await _moviesService.GetMovieByTitleAsync(new GetMovieByTitleRequest(title, year));

                return response.Match((result) =>
                {
                    return Ok(result.Movies);
                }, (error) =>
                {
                    return StatusCode(error.StatusCode, new { error.ErrorMessages });
                });

            }, "Get movie by title and/or year.");
        }

    }
}
