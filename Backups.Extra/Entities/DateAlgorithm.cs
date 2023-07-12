using Backups.Entities;
using Backups.Extra.Interfaces;

namespace Backups.Extra.Entities;

public class DateAlgorithm : IRestorePointLimitAlgorithm
{
    public DateAlgorithm(Backup backup, DateTime dateTime)
    {
        ArgumentNullException.ThrowIfNull(backup);
        ArgumentNullException.ThrowIfNull(dateTime);
        Backup = backup;
        DateTime = dateTime;
    }

    public Backup Backup { get; private set; }
    public DateTime DateTime { get; private set; }
    public List<RestorePoint> FindAllRestorePoints()
    {
        var currentList = new List<RestorePoint>();
        currentList = Backup.GetRestorePoints()
            .Where(x => x.DateTime < DateTime)
            .ToList();
        return currentList;
    }
}