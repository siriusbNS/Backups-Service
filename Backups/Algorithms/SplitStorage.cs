using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;
using Zio;

namespace Backups.Algorithms;

public class SplitStorage : IAlgorithm
{
    private int _id = 1;
    public SplitStorage(IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;
    }

    public IRepository Repository { get; private set; }
    public List<Storage> RunAlgorithm(List<BackupObject> list, RestorePoint restorePoint, int restorePointId, string nameOfBackup)
    {
        Repository.CreateDirectory(Repository.GetPath() / nameOfBackup / restorePoint.NameOfRestorePoint);
        var currentListOfRP = new List<Storage>();
        foreach (var i in list)
        {
            _id++;
            Repository.ArchiveFileInZip(i.NameOfBackupObj, (Repository.GetPath() / nameOfBackup / restorePoint.NameOfRestorePoint / i.NameOfBackupObj).ToString() + ".zip");
            currentListOfRP.Add(new Storage(Repository, i.NameOfBackupObj + ".zip"));
        }

        return currentListOfRP;
    }
}