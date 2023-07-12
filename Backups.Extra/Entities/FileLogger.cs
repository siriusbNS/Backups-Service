using System.Text;
using Backups.Extra.Interfaces;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public class FileLogger : ILogger
{
    public FileLogger(IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;
        Repository.OpenFile((Repository.GetPath() / "FileLogger").ToString());
    }

    private IRepository Repository { get; set; }

    public void Log(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        File.WriteAllText((Repository.GetPath() / "FileLogger").ToString(), message, Encoding.Default);
    }
}