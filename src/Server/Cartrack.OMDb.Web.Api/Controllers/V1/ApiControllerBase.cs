using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly ILogger<ApiControllerBase> _logger;

        protected ApiControllerBase(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiControllerBase>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func">Encapsulated method to perform the <paramref name="operation"/></param>
        /// <param name="operation">Friendly name for the operation to be performed</param>
        /// <returns></returns>
        protected async Task<IActionResult> InvokeAppServiceAsync(Func<Task<IActionResult>> func, string operation)
        {
            try
            {
                return await func();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unexpected error occurred and caused the operation '{operation}' to fail. See exception for more details.", operation);

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpecterd error occurred and caused the operation to fail.");
            }
        }

    }
}