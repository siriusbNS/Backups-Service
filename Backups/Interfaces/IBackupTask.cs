using Backups.Entities;
using Zio;

namespace Backups.Interfaces;

public interface IBackupTask
{
    List<Storage> RunTask();
    public IReadOnlyList<BackupObject> GetBackupObjects();
}