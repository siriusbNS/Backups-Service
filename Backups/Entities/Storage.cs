using Backups.Interfaces;
using Newtonsoft.Json;
using Zio;

namespace Backups.Entities;

public class Storage
{
    public Storage(IRepository repository, string name)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(name);
        Repository = repository;
        NameOfStorage = name;
    }

    [JsonProperty("name")]
    public string NameOfStorage { get; private set; }
    [JsonProperty("repository")]
    private IRepository Repository { get; set; }
}