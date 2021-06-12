using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Cartrack.OMDb.Web.Api.Bootstrap.OpenApi
{
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.SingleOrDefault(param => param.Name.Equals("api-version", StringComparison.InvariantCultureIgnoreCase));
            operation.Parameters.Remove(versionParameter);
        }
    }
}
