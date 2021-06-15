using Cartrack.OMDb.Clients.Cli.Models;
using Cartrack.OMDb.Clients.Cli.Options;
using Cartrack.OMDb.Clients.Cli.Services;
using CommandLine;
using OneOf;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Clients.Cli
{
    public class CartrackOMDbApplication
    {
        private readonly IConsoleWriter _console;
        private readonly ITitlesService _titlesService;
        private readonly JsonSerializerOptions _serializerOptions;

        public CartrackOMDbApplication(IConsoleWriter console, ITitlesService titlesService)
        {
            _console = console;
            _titlesService = titlesService;
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public async Task<int> RunAsync(string[]  args)
        {
            return Parser.Default
                .ParseArguments<GetTitleOptions, SearchTitlesOptions, QueryCacheOptions, CreateCacheEntryOptions, UpdateCacheEntryOptions, DeleteCacheEntryOptions>(args)
                .MapResult(
                    (GetTitleOptions options) => GetTitleAsync(options),
                    (SearchTitlesOptions options) => SearchTitlesAsync(options),
                    (QueryCacheOptions options) => QueryCacheEntriesAsync(options),
                    (CreateCacheEntryOptions options) => CreateCacheEntryAsync(options),
                    (UpdateCacheEntryOptions options) => UpdateCacheEntryAsync(options),
                    (DeleteCacheEntryOptions options) => DeleteCacheEntryAsync(options),
                    (error) => 1);
        }

        public int GetTitleAsync(GetTitleOptions options)
        {
            var result = _titlesService.GetTitleByIMDbIdAsync(new TitleRequest(options.IMDbID))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            HandleResults(result);

            return 0;
        }

        private int SearchTitlesAsync(SearchTitlesOptions options)
        {
            var result = _titlesService.SearchTitlesAsync(new SearchTitlesRequest(options.SearchTerm, options.Year))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            HandleResults(result);

            return 0;
        }

        private int QueryCacheEntriesAsync(QueryCacheOptions options)
        {
            var result = _titlesService.QueryCacheEntryAsync(new QueryTitlesRequest(options.Select, options.Where, options.OrderBy, options.Count))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            HandleResults(result);

            return 0;
        }

        private int CreateCacheEntryAsync(CreateCacheEntryOptions options)
        {
            var result = _titlesService.CreateCacheEntryAsync(new CreateOrUpdateTitleRequest(options.IMDbID, options.Name, options.Year, options.Type))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            result.Switch((result) =>
            {
                var formattedJsonResult = JsonSerializer.Serialize(result, _serializerOptions);
                _console.WriteLine("Title added to cache.");
                _console.WriteLine(formattedJsonResult, ConsoleColor.Green);
            }, (error) =>
            {
                var formattedErrors = string.Join(Environment.NewLine, error.ErrorMessages);
                _console.WriteLine(formattedErrors, ConsoleColor.Red);
            });

            return 0;
        }

        private int UpdateCacheEntryAsync(UpdateCacheEntryOptions options)
        {
            var result = _titlesService.UpdateCacheEntryAsync(new CreateOrUpdateTitleRequest(options.IMDbID, options.Name, options.Year, options.Type))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            result.Switch((result) =>
            {
                var formattedJsonResult = JsonSerializer.Serialize(result, _serializerOptions);
                _console.WriteLine("Title cache entry updated.");
                _console.WriteLine(formattedJsonResult, ConsoleColor.Green);
            }, (error) =>
            {
                var formattedErrors = string.Join(Environment.NewLine, error.ErrorMessages);
                _console.WriteLine(formattedErrors, ConsoleColor.Red);
            });

            return 0;
        }

        private int DeleteCacheEntryAsync(DeleteCacheEntryOptions options)
        {
            var result = _titlesService.DeleteCacheEntryAsync(new TitleRequest(options.IMDbID))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            result.Switch((deleted) =>
            {
                if (deleted)
                {
                    _console.WriteLine($"Cache entry {options.IMDbID} deleted", ConsoleColor.Yellow);
                }
                else
                {
                    _console.WriteLine($"Failed to delete cache entry {options.IMDbID}. Please try again later.", ConsoleColor.Red);
                }

            }, (error) =>
            {
                var formattedErrors = string.Join(Environment.NewLine, error.ErrorMessages);
                _console.WriteLine(formattedErrors, ConsoleColor.Red);
            });

            return 0;
        }

        private void HandleResults<TResult, TError>(OneOf<TResult, TError> result) where TError : TitleErrorResult
        {
            result.Switch((result) =>
            {
                var formattedJsonResult = JsonSerializer.Serialize(result, _serializerOptions);
                _console.WriteLine(formattedJsonResult, ConsoleColor.Green);
            }, (error) =>
            {
                var formattedErrors = string.Join(Environment.NewLine, error.ErrorMessages);
                _console.WriteLine(formattedErrors, ConsoleColor.Red);
            });
        }
    }
}