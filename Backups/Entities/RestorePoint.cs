using Newtonsoft.Json;
using Zio;

namespace Backups.Entities;

public class RestorePoint
{
    public RestorePoint(List<BackupObject> backupObjects, string name)
    {
        ArgumentNullException.ThrowIfNull(backupObjects);
        ArgumentNullException.ThrowIfNull(name);
        ListOfStorages = new List<Storage>();
        ListOfBAckupObjects = backupObjects;
        DateTime = DateTime.Now;
        NameOfRestorePoint = name;
    }

    [JsonProperty("name")]
    public string NameOfRestorePoint { get; private set; }
    public DateTime DateTime { get; private set; }
    public IReadOnlyList<Storage> Storages => ListOfStorages;
    public IReadOnlyList<BackupObject> BackupObjects => ListOfBAckupObjects;
    private List<BackupObject> ListOfBAckupObjects { get; set; }
    private List<Storage> ListOfStorages { get; set; }
    public IReadOnlyList<BackupObject> GetBackupObjects() => ListOfBAckupObjects;
    public IReadOnlyList<Storage> GetStorages() => ListOfStorages;

    public void AddBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        ListOfBAckupObjects.Add(backupObject);
    }

    public void AddStorage(Storage storage)
    {
        ArgumentNullException.ThrowIfNull(storage);
        ListOfStorages.Add(storage);
    }
}