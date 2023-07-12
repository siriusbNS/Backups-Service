using Backups.Extra.Interfaces;

namespace Backups.Extra.Entities;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Console.WriteLine(message);
    }
}