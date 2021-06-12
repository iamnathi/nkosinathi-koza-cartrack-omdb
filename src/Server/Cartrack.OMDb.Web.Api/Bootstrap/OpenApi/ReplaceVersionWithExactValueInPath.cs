using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cartrack.OMDb.Web.Api.Bootstrap.OpenApi
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var apiPaths = new OpenApiPaths();
            foreach (var docPath in swaggerDoc.Paths)
            {
                apiPaths.Add(docPath.Key.Replace("v{version}", swaggerDoc.Info.Version), docPath.Value);
            }

            swaggerDoc.Paths = apiPaths;
        }
    }
}