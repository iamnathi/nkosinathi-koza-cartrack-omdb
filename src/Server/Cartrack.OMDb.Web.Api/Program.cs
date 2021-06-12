using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Repositories;
using Cartrack.OMDb.Web.Api.Bootstrap.Caching;
using Cartrack.OMDb.Web.Models.Results.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                using (var host = CreateHostBuilder(args).Build())
                {
                    Log.Information("Seeding Cache from DB...");
                    var movieRepository = host.Services.GetRequiredService<IMovieRespository>();
                    var cacheProvider = host.Services.GetRequiredService<ICacheProvider<MovieResult>>();
                    await MovieCacheSeed.EnsureCacheSeed(movieRepository, cacheProvider);
                    Log.Information("Finished seeding Cache from DB...");

                    await host.RunAsync();
                }
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = false;
                });
        }
    }
}
