using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Application.Services;
using Cartrack.OMDb.Web.Models.Results.Models;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationsServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<ITitleService, TitleService>();
            services.AddSingleton<ICacheProvider<TitleResult>, InMemoryCacheProvider<TitleResult>>();

            return services;
        }
    }
}
