using Backups.Entities;
using Zio;

namespace Backups.Interfaces;

public interface IAlgorithm
{
    List<Storage> RunAlgorithm(List<BackupObject> list, RestorePoint restorePoint, int restorePointId, string nameOfBackup);
}