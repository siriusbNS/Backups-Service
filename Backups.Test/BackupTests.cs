using Backups.Algorithms;
using Backups.Entities;
using Backups.Interfaces;
using Xunit;
using Zio;

namespace Backups.Test;

public class BackupTests
{
    [Fact(Skip = "LocalFS")]
    public void Test1()
    {
        IRepository repository = new RepoPhysicalFs("/Users/siriusa/Desktop/Repo5");
        BackupTask backupTask = new BackupTask(repository, new SingleStorage(repository), "Backup1");
        repository.OpenFile("testFile.txt");
        repository.OpenFile("testFile2.txt");
        var newBackupObj = new BackupObject("testFile.txt");
        var newBackupObj2 = new BackupObject("testFile2.txt");
        backupTask.AddBackupObject(newBackupObj);
        backupTask.AddBackupObject(newBackupObj2);
        var currentList = backupTask.RunTask();
        backupTask.DeleteBackupObject(newBackupObj);
        var currentList2 = backupTask.RunTask();
        int valueOfStorage = 2;
        int valueOfRP = 2;
        Assert.Equal(valueOfStorage, currentList.Count + currentList2.Count);
        Assert.Equal(valueOfRP, backupTask.FindRPInBackup().Count);
    }

    [Fact]
    public void Test2()
    {
        IRepository repository = new RepoInMemoryFs("/home/user/Repo6");
        BackupTask backupTask = new BackupTask(repository, new SingleStorage(repository), "Backup1");
        repository.OpenFile("testFile.txt");
        repository.OpenFile("testFile2.txt");
        var newBackupObj = new BackupObject("testFile.txt");
        var newBackupObj2 = new BackupObject("testFile2.txt");
        backupTask.AddBackupObject(newBackupObj);
        backupTask.AddBackupObject(newBackupObj2);
        var currentStorage = backupTask.RunTask()[0];
        Assert.Equal(2, backupTask.GetBackupObjects().Count);
        Assert.Equal(2, backupTask.AddRestorePoint().GetBackupObjects().Count);
    }
}