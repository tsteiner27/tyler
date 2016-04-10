using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TylerSteiner.Cli.CommandLine;
using TylerSteiner.Cli.Commands;
using TylerSteiner.Core.Options;
using TylerSteiner.Services.Data;

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

            // Services
            services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

            // Commands
            services.AddTransient<ImportExcelFileCommand>();

            // CLI
            var console = AnsiConsole.GetOutput(useConsoleColor: true);
            services.AddInstance(console);

            return services.BuildServiceProvider();
        }
    }
}