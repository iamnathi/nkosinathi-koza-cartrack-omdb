using Cartrack.OMDb.Clients.Cli;
using Cartrack.OMDb.Clients.Cli.API;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using Cartrack.OMDb.Clients.Cli.Models;

namespace SimpleInjector
{
    public static class RefitContinerExtensions
    {
        /// <summary>
        /// I wanted to take advantage of the code-generation feature of Refit
        /// but registering IHttpClientFacotry seemed to be trivial as DefaultHttpClientFactory has an internal scope
        /// 
        /// </summary>
        /// <param name="contianer">DI Container</param>
        /// <returns></returns>
        public static Container RegisterConfiguredRefit(this Container container, IConfiguration configuration)
        {
            if (container == null)
            {
                throw new ArgumentNullException();
            }

            var services = new ServiceCollection();
            services.AddRefitClient<ICartrackOMDbApi>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(configuration["CartrackOMDbAPI:BaseAddress"]);
                });

            services.AddValidatorsFromAssemblyContaining<Program>();

            var provider = services.BuildServiceProvider();
            container.RegisterInstance(provider.GetService<ICartrackOMDbApi>());
            
            container.RegisterInstance(provider.GetService<IValidator<TitleRequest>>());
            container.RegisterInstance(provider.GetService<IValidator<SearchTitlesRequest>>());
            container.RegisterInstance(provider.GetService<IValidator<CreateOrUpdateTitleRequest>>());

            container.ContainerScope.RegisterForDisposal(provider);

            return container;
        }
    }
}
