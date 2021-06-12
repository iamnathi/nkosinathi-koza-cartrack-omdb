using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public class SwaggerOptionsConfigure : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public SwaggerOptionsConfigure(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var apiDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(apiDescription.GroupName, CreateOpenApiVersionInfo(apiDescription));                
            }
        }

        private OpenApiInfo CreateOpenApiVersionInfo(ApiVersionDescription apiVersionDescription)
        {
            var info = new OpenApiInfo
            {
                Title = "Cartrack OMDb API",
                Version = apiVersionDescription.ApiVersion.ToString(),
                Contact = new OpenApiContact
                {
                    Name = "Nkosinathi Koza",
                    Email = "kozankosinathi@gmail.com"
                }
            };

            if (apiVersionDescription.IsDeprecated)
            {
                info.Description += " (This API version has been deprecated)";
            }

            return info;
        }
    }
}
