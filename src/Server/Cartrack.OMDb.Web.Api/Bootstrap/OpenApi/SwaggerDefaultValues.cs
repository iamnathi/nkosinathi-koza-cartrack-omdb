using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;
using System;
using Microsoft.OpenApi.Any;

namespace Cartrack.OMDb.Web.Api.Bootstrap.OpenApi
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.FirstOrDefault(param => param.Name.Equals(parameter.Name, StringComparison.InvariantCultureIgnoreCase));
                if (description != null)
                {
                    parameter.Description ??= description.ModelMetadata?.Description;

                    if (parameter.Schema.Default == null && description.DefaultValue != null)
                    {
                        parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                    }

                    parameter.Required |= description.IsRequired;
                }                
            }
            
        }
    }
}