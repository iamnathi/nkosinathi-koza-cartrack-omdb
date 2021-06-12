using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class OpenApiApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var apiDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{apiDescription.GroupName}/swagger.json", apiDescription.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}
