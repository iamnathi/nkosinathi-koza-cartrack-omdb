using System;

namespace Cartrack.OMDb.Clients.Cli.Services
{
    public interface IConsoleWriter
    {
        void WriteLine(string text, ConsoleColor foregroundColor = ConsoleColor.White);
    }
}
