using System;

namespace TylerSteiner.Cli.CommandLine
{
    internal static class CommandLineExtensions
    {
        public static T ValueAs<T>(this CommandArgument argument)
        {
            return (T)Convert.ChangeType(argument.Value, typeof (T));
        }
    }
}