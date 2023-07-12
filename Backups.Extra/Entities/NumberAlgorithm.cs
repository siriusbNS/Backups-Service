using Backups.Entities;
using Backups.Extra.Interfaces;

namespace Backups.Extra.Entities;

public class NumberAlgorithm : IRestorePointLimitAlgorithm
{
    public NumberAlgorithm(Backup backup, int number)
    {
        ArgumentNullException.ThrowIfNull(backup);
        ArgumentNullException.ThrowIfNull(number);
        Backup = backup;
        Number = number;
    }

    public Backup Backup { get; private set; }
    public int Number { get; private set; }
    public List<RestorePoint> FindAllRestorePoints()
    {
        int currentValue = 0;
        if (Number < Backup.GetRestorePoints().Count)
            currentValue = Backup.GetRestorePoints().Count - Number;
        if (Number >= Backup.GetRestorePoints().Count)
            currentValue = Backup.GetRestorePoints().Count;

        return Backup
            .GetRestorePoints()
            .Where((x, i) => i < currentValue && x == Backup.GetRestorePoints()[i])
            .ToList();
    }
}