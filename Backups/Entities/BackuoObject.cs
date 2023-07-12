using Newtonsoft.Json;
using Zio;

namespace Backups.Entities;

public class BackupObject
{
    public BackupObject(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        NameOfBackupObj = name;
    }

    [JsonProperty("name")]
    public string NameOfBackupObj { get; private set; }
}