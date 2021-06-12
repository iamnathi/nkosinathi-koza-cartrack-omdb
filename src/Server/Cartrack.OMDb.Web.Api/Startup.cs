using Cartrack.OMDb.Application.Configurations;
using Cartrack.OMDb.Web.Api.Bootstrap.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly.Extensions.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Microsoft.AspNet.OData.Extensions;
using Polly;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using FluentValidation.AspNetCore;
using Cartrack.OMDb.Application.Validators;

namespace Cartrack.OMDb.Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers(options =>
            {
                options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsConfigure>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<RemoveVersionFromParameter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                options.IncludeXmlComments(XmlDocumentationPath);
                options.CustomSchemaIds(s => s.FullName);
                options.DocInclusionPredicate((version, description) =>
                {
                    if (!description.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = methodInfo
                        .GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versions.Any(ver => $"v{ver}" == version && (!maps.Any() || maps.Any(ver => $"v{ver}" == version)));
                });
            });

            services.AddApplicationServices();
            services.AddRepositories();
            services.AddOData();

            services.Configure<OmdbApiSettings>(Configuration.GetSection(nameof(OmdbApiSettings)));
            services.AddHttpClient(StringConstants.OmdbApiClientName, (provider, httpClient) =>
            {
                var omdbApiSettings = provider.GetRequiredService<IOptions<OmdbApiSettings>>().Value;

                httpClient.BaseAddress = new Uri(omdbApiSettings.BaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .RetryAsync(3, (exception, retryCount) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{retryCount}]: Retrying HTTP Call to OMDb API due to: {exception.Exception.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }));

            services.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<GetMovieByIdRequestValidator>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseConfiguredSwagger(apiVersionDescriptionProvider);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static string XmlDocumentationPath
        {
            get
            {
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                return Path.Combine(AppContext.BaseDirectory, xmlFile);
            }
        }
    }
}
