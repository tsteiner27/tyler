using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TylerSteiner.Cli.CommandLine;
using TylerSteiner.Services.Data;

namespace TylerSteiner.Cli.Commands
{
    internal class ImportExcelFileCommand
    {
        public static Action<CommandLineApplication> Configure(IServiceProvider provider, CancellationToken cancel)
        {
            return command =>
            {
                command.Description = "Imports the excel file";
                command.HelpOption("-?|-h|--help");

                command.OnExecute(async () => await provider.GetService<ImportExcelFileCommand>().ExecuteAsync(cancel));
            };
        }

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly AnsiConsole _console;

        public ImportExcelFileCommand(IDbConnectionFactory connectionFactory, AnsiConsole console)
        {
            _connectionFactory = connectionFactory;
            _console = console;
        }

        private Task<int> ExecuteAsync(CancellationToken token)
        {
            _console.WriteLine("Hello, world");

            return Task.FromResult(1);
        }
    }
}