using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;
using Zio;

namespace Backups.Algorithms;

public class SingleStorage : IAlgorithm
{
    public SingleStorage(IRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        Repository = repository;
    }

    public IRepository Repository { get; private set; }
    public List<Storage> RunAlgorithm(List<BackupObject> list, RestorePoint restorePoint, int restorePointId, string nameOfBackup)
    {
        Repository.CreateDirectory(Repository.GetPath() / nameOfBackup / restorePoint.NameOfRestorePoint);
        var currentListOfRP = new List<Storage>();
        currentListOfRP.Add(new Storage(Repository,  "Storage.zip"));
        foreach (var i in list)
        {
            Repository.ArchiveFileInZip(i.NameOfBackupObj, (Repository.GetPath() / nameOfBackup / restorePoint.NameOfRestorePoint / "Storage.zip").ToString());
        }

        return currentListOfRP;
    }
}