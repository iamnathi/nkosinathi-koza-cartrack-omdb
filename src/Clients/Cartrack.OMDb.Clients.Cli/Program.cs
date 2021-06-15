using Cartrack.OMDb.Clients.Cli.Extensions;
using Cartrack.OMDb.Clients.Cli.Services;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Clients.Cli
{
    public class Program
    {
        private static Container _container;

        public static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();

            BuildContainer(configuration);
            ConfigureServices(_container, configuration);

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                var app = _container.GetInstance<CartrackOMDbApplication>();
                await app.RunAsync(args.ParseWithSpace());
            }
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
        }

        private static void BuildContainer(IConfiguration configuration)
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
            _container.RegisterInstance(configuration);
        }

        public static void ConfigureServices(Container container, IConfiguration configuration)
        {
            container.RegisterSingleton<CartrackOMDbApplication>();
            container.RegisterSingleton<IConsoleWriter, ConsoleWriter>();
            container.RegisterSingleton<ITitlesService, TitlesService>();
            container.RegisterConfiguredRefit(configuration);
            container.Verify();
        }
    }
}
