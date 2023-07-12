using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public class MergeAlgorithm
{
    public MergeAlgorithm()
    {
    }

    public RestorePoint MergeRestorePoints(Backup backup, IRepository repository, string nameOfBackupTask)
    {
        foreach (var i in backup.GetRestorePoints().ToList())
        {
            if (File.Exists((repository.GetPath() / nameOfBackupTask / i.NameOfRestorePoint / "Storage.zip")
                    .ToString()))
            {
                var currentRP = backup.GetRestorePoints()
                    .FirstOrDefault(x => x.NameOfRestorePoint == i.NameOfRestorePoint);
                ArgumentNullException.ThrowIfNull(currentRP);
                backup.RemoveRestorePoint(currentRP);
                repository.DeleteFile(repository.GetPath() / nameOfBackupTask / i.NameOfRestorePoint / "Storage.zip");
            }
        }

        var currentListOfRPToDelete = new List<RestorePoint>();
        for (int i = 0; i < backup.GetRestorePoints().Count - 1; i++)
        {
            var restorePointFirst = backup.GetRestorePoints()[i];
            var restorePointSecond = backup.GetRestorePoints()[i + 1];
            ArgumentNullException.ThrowIfNull(restorePointFirst);
            ArgumentNullException.ThrowIfNull(restorePointSecond);
            restorePointFirst.GetBackupObjects()
                .Where(j => restorePointSecond.GetBackupObjects()
                                .FirstOrDefault(x => x.NameOfBackupObj == j.NameOfBackupObj) ==
                            null)
                .Select(obj =>
                {
                    restorePointSecond.AddBackupObject(obj);
                    repository.CopyFile(
                        repository.GetPath() / nameOfBackupTask / restorePointFirst.NameOfRestorePoint /
                        (obj.NameOfBackupObj + ".zip"), repository.GetPath() / nameOfBackupTask / restorePointSecond.NameOfRestorePoint / (obj.NameOfBackupObj + ".zip"));
                    return obj;
                })
                .ToList();
            currentListOfRPToDelete.Add(restorePointFirst);
        }

        int count = currentListOfRPToDelete.Count;
        foreach (var i in currentListOfRPToDelete)
        {
                backup.RemoveRestorePoint(i);
                repository.DeleteDirectory(repository.GetPath() / nameOfBackupTask / i.NameOfRestorePoint);
        }

        return backup.GetRestorePoints().First();
    }
}