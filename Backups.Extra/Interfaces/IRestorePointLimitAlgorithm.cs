using Backups.Entities;

namespace Backups.Extra.Interfaces;

public interface IRestorePointLimitAlgorithm
{
    List<RestorePoint> FindAllRestorePoints();
}