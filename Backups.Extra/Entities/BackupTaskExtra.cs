using System.IO.Compression;
using System.Text;
using Backups.Entities;
using Backups.Extra.Interfaces;
using Newtonsoft.Json;

namespace Backups.Extra.Entities;

public class BackupTaskExtra
{
    public BackupTaskExtra(BackupTask backupTask, IRestorePointLimitAlgorithm restorePointLimitAlgorithm, ILogger logger, MergeAlgorithm mergeAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(backupTask);
        ArgumentNullException.ThrowIfNull(restorePointLimitAlgorithm);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(mergeAlgorithm);
        BackupTask = backupTask;
        RestorePointLimitAlgorith = restorePointLimitAlgorithm;
        Logger = logger;
        MergeAlgorithm = mergeAlgorithm;
    }

    private BackupTask BackupTask { get; set; }
    private IRestorePointLimitAlgorithm RestorePointLimitAlgorith { get; set; }
    private ILogger Logger { get; set; }
    private MergeAlgorithm MergeAlgorithm { get; set; }
    public Backup GetBackup() => BackupTask.Backup;
    public void AddBackupObject(BackupObject backupObject)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        BackupTask.AddBackupObject(backupObject);
    }

    public void RunTask()
    {
        var list = BackupTask.RunTask();
        string message = "The " + $"{BackupTask.Backup.GetRestorePoints()[BackupTask.Backup.GetRestorePoints().Count - 1].NameOfRestorePoint}" + " was created! : " + $"{DateTime.UtcNow.ToString()}";
        Logger.Log(message);
        foreach (var i in list)
        {
            message = "The " + $" The {i.NameOfStorage}" + " was created! : " + $"{DateTime.UtcNow.ToString()}";
            Logger.Log(message);
        }
    }

    public void CleanRestorePoints()
    {
        var list = RestorePointLimitAlgorith.FindAllRestorePoints();
        foreach (var i in list)
        {
            var currentRP = BackupTask.Backup.GetRestorePoints()
                .FirstOrDefault(x => x.NameOfRestorePoint == i.NameOfRestorePoint);
            ArgumentNullException.ThrowIfNull(currentRP);
            BackupTask.Backup.RemoveRestorePoint(currentRP);
            BackupTask.Repository.DeleteDirectory(BackupTask.Repository.GetPath() / BackupTask.NameOfBackupTask / i.NameOfRestorePoint);
        }
    }

    public void MergeRestorePoints()
    {
        MergeAlgorithm.MergeRestorePoints(BackupTask.Backup, BackupTask.Repository, BackupTask.NameOfBackupTask);
    }

    public void ReestablishRestorePoint(string nameOfRP)
    {
        var currentRP = BackupTask.Backup.GetRestorePoints()
            .FirstOrDefault(x => x.NameOfRestorePoint == nameOfRP);
        ArgumentNullException.ThrowIfNull(currentRP);
        foreach (var i in currentRP.GetStorages())
        {
            var stream = BackupTask.Repository.OpenFile((BackupTask.Repository.GetPath() / BackupTask.NameOfBackupTask / currentRP.NameOfRestorePoint / i.NameOfStorage).ToString());
            var content = new StreamReader(stream);
            var contentString = content.ReadToEnd();
            File.WriteAllText((BackupTask.Repository.GetPath() / i.NameOfStorage).ToString() + "2.0", contentString);
        }
    }

    public void Save(string path)
    {
        string jsonContent = JsonConvert.SerializeObject(BackupTask, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
        });
        var stream = BackupTask.Repository.OpenFile(path);
        File.WriteAllText(path, jsonContent);
        stream.Write(Encoding.ASCII.GetBytes(jsonContent));
    }

    public BackupTask Load(string path)
    {
        var stream = BackupTask.Repository.OpenFile(path);
        var content = new StreamReader(stream);
        var contentString = content.ReadToEnd();
        var currentBackupTask = JsonConvert.DeserializeObject<BackupTask>(
            contentString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            });
        ArgumentNullException.ThrowIfNull(currentBackupTask);
        return currentBackupTask;
    }
}