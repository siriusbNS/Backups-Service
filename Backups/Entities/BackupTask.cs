using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;
using Zio;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    [JsonConstructor]
    public BackupTask(IRepository repository, IAlgorithm algorithm, string nameOfTask, Backup backup, int id, List<BackupObject> listOfBAckupObjects)
    {
        Algorithm = algorithm;
        Repository = repository;
        ListOfBAckupObjects = listOfBAckupObjects;
        Backup = backup;
        Id = id;
        NameOfBackupTask = nameOfTask;
        Repository.CreateDirectory(Repository.GetPath() / nameOfTask);
    }

    public BackupTask(IRepository repository, IAlgorithm algorithm, string nameOfTask)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(algorithm);
        Algorithm = algorithm;
        Repository = repository;
        ListOfBAckupObjects = new List<BackupObject>();
        Backup = new Backup();
        Id = 0;
        NameOfBackupTask = nameOfTask;
        Repository.CreateDirectory(Repository.GetPath() / nameOfTask);
    }

    public int Id { get; private set; }
    [JsonProperty("nameOfTask")]
    public string NameOfBackupTask { get; private set; }
    public Backup Backup { get; private set; }
    public IRepository Repository { get; set; }
    public IReadOnlyList<BackupObject> BackupObjects => ListOfBAckupObjects;
    [JsonProperty("algorithm")]
    private IAlgorithm Algorithm { get; set; }
    private List<BackupObject> ListOfBAckupObjects { get; set; }
    public IReadOnlyList<BackupObject> GetBackupObjects() => ListOfBAckupObjects;
    public List<RestorePoint> FindRPInBackup()
    {
        return Backup.GetRestorePoints().ToList();
    }

    public void AddBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        ListOfBAckupObjects.Add(backupObject);
    }

    public void DeleteBackupObject(BackupObject backuoObject)
    {
        ArgumentNullException.ThrowIfNull(backuoObject);
        ListOfBAckupObjects.Remove(backuoObject);
    }

    public RestorePoint AddRestorePoint()
    {
        return Backup.AddRestorePoint(GetBackupObjects().ToList(), Id);
    }

    public List<Storage> RunTask()
    {
        Id++;
        Repository.CreateDirectory(Repository.GetPath() / NameOfBackupTask);
        RestorePoint currentRP = AddRestorePoint();
        AddBackupObjectsToBackupHistory(GetBackupObjects().ToList());
        var currentList = Algorithm.RunAlgorithm(ListOfBAckupObjects, currentRP, Id, NameOfBackupTask);
        foreach (var i in currentList)
        {
            currentRP.AddStorage(i);
        }

        return currentList;
    }

    private void AddBackupObjectsToBackupHistory(List<BackupObject> list)
    {
        if (ListOfBAckupObjects.Count is 0)
            throw new ListOfBackupObjEmptyException("There are no backup objects.");
        list.ForEach(i => Backup.AddBackupObject(i, Id));
    }
}