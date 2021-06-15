using System;

namespace Cartrack.OMDb.Clients.Cli.Services
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteLine(string text, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}