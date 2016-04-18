using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TylerSteiner.Cli.CommandLine;
using TylerSteiner.Cli.Commands;
using TylerSteiner.Core.Options;
using TylerSteiner.Services;
using TylerSteiner.Services.Data;
using TylerSteiner.Services.Internal;
using TylerSteiner.Services.Providers;

namespace TylerSteiner.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => cts.Cancel();

            var provider = BuildProvider();

            var app = new CommandLineApplication
            {
                Name = "dnx ts",
                FullName = "Tyler Steiner Movies Application CLI",
            };

            app.HelpOption("-?|-h|--help");
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            app.Command("import-excel", ImportExcelFileCommand.Configure(provider, cts.Token));

            app.Execute(args);
        }

        private static IServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            
            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("secrets.json", false)
                .Build();

            services.AddOptions();
            services.Configure<DatabaseConfig>(configuration.GetSection("Database"));
            
            // Logging
            services.AddLogging();
            
            // Services
            services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddSingleton<IJsonParser, JsonParser>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<ISqlQueryFactory, SqlQueryFactory>();
            services.AddTransient<IOmdbMovieDataService, OmdbMovieDataService>();
            services.AddTransient<IImdbMovieDataService, ImdbMovieDataService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IMovieImportService, MovieImportService>();

            // Commands
            services.AddTransient<ImportExcelFileCommand>();

            // CLI
            var console = AnsiConsole.GetOutput(useConsoleColor: true);
            services.AddInstance(console);

            var provider = services.BuildServiceProvider();

            // Logging
            var loggerFactory = provider.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(configuration.GetSection("Logging"));

            return provider;
        }
    }
}