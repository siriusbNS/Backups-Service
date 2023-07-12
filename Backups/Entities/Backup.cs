using Backups.Interfaces;
using Backups.Tools;
using Newtonsoft.Json;
using Zio;

namespace Backups.Entities;

public class Backup
{
    public Backup()
    {
        ListOfRestorePoints = new List<RestorePoint>();
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
    public IReadOnlyList<RestorePoint> RestorePoints => ListOfRestorePoints;
    private List<RestorePoint> ListOfRestorePoints { get; set; }
    public IReadOnlyList<RestorePoint> GetRestorePoints() => ListOfRestorePoints;
    public void AddBackupObject(BackupObject backupObject, int idOfRP)
    {
        ArgumentNullException.ThrowIfNull(backupObject);
        var currentRP = ListOfRestorePoints
            .FirstOrDefault(x => x.NameOfRestorePoint == "RestorePoint" + idOfRP.ToString());
        ArgumentNullException.ThrowIfNull(currentRP);
        currentRP.AddBackupObject(backupObject);
    }

    public RestorePoint AddRestorePoint(List<BackupObject> list, int restorePointId)
    {
        if (restorePointId < 0)
        {
            throw new RestorePointException("uncorrect Id.");
        }

        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(restorePointId);
        var currentRP = new RestorePoint(list, "RestorePoint" + restorePointId.ToString());
        ListOfRestorePoints.Add(currentRP);
        return currentRP;
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        var currentRP = ListOfRestorePoints
            .FirstOrDefault(x => x.NameOfRestorePoint == restorePoint.NameOfRestorePoint);
        ArgumentNullException.ThrowIfNull(currentRP);
        ArgumentNullException.ThrowIfNull(restorePoint);
        ListOfRestorePoints.Remove(currentRP);
    }
}