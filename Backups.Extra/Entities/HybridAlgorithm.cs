using Backups.Entities;
using Backups.Extra.Enums;
using Backups.Extra.Interfaces;
using Backups.Extra.Tools;

namespace Backups.Extra.Entities;

public class HybridAlgorithm : IRestorePointLimitAlgorithm
{
    public HybridAlgorithm(Backup backup, List<IRestorePointLimitAlgorithm> list, HybridMode hybridMode)
    {
        ArgumentNullException.ThrowIfNull(backup);
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(hybridMode);
        if (list.FirstOrDefault(x => x is HybridAlgorithm) != null)
            throw new Exception();
        Backup = backup;
        LimitAlgorithms = list;
        HybridMode = hybridMode;
    }

    public Backup Backup { get; private set; }
    public List<IRestorePointLimitAlgorithm> LimitAlgorithms { get; private set; }
    public HybridMode HybridMode { get; private set; }
    public List<RestorePoint> FindAllRestorePoints()
    {
        var currentList = new List<RestorePoint>();
        var currentListSecond = new List<RestorePoint>();
        if (HybridMode is HybridMode.One)
        {
            foreach (var j in LimitAlgorithms.SelectMany(i => i.FindAllRestorePoints().Where(j => currentList.FirstOrDefault(x => x.NameOfRestorePoint == j.NameOfRestorePoint) == null)))
            {
                currentList.Add(j);
            }

            return currentList;
        }

        if (HybridMode is HybridMode.All)
        {
            foreach (var j in LimitAlgorithms.SelectMany(i => i.FindAllRestorePoints()))
            {
                currentList.Add(j);
                if (currentList.Where(x => x.NameOfRestorePoint == j.NameOfRestorePoint).ToList().Count == 2)
                {
                    currentListSecond.Add(j);
                }
            }

            return currentListSecond;
        }

        throw new HybridModeException("There is no this mode");
    }
}