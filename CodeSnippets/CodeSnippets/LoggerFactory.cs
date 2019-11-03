using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    public enum LoggerType { Console, Database };

    public class LoggerFactory
    {
        public ILogger GetLogger(LoggerType loggerType) => loggerType switch
        {
            LoggerType.Console => new ConsoleLogger(),
            LoggerType.Database => new DatabaseLogger(),
            _ => throw new ArgumentException(message: "Not known logger type", paramName: nameof(loggerType))
        };
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        void ILogger.Log(string message)
        {
            Console.WriteLine($"Displayed on console: {message}");
        }
    }

    public class DatabaseLogger : ILogger
    {
        void ILogger.Log(string message)
        {
            Console.WriteLine($"Written to database: {message}");
        }
    }
}
