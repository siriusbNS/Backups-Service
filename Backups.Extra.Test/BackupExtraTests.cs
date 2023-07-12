using Backups.Algorithms;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Interfaces;
using Backups.Interfaces;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using Zio;

namespace Backups.Extra.Test;

public class BackupExtraTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BackupExtraTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact(Skip = "local FS")]
    public void Test1()
    {
        IRepository repository = new RepoPhysicalFs("/Users/siriusa/Desktop/Repo5");
        BackupTask backupTask = new BackupTask(repository, new SplitStorage(repository), "Backup1");
        repository.OpenFile("testFile.txt");
        repository.OpenFile("testFile2.txt");
        repository.OpenFile("json.txt");
        var newBackupObj = new BackupObject("testFile.txt");
        var newBackupObj2 = new BackupObject("testFile2.txt");
        backupTask.AddBackupObject(newBackupObj);
        backupTask.AddBackupObject(newBackupObj2);
        var currentList = backupTask.RunTask();
        var currentList2 = backupTask.RunTask();
        backupTask.RunTask();
        backupTask.RunTask();
        ILogger logger = new ConsoleLogger();
        IRestorePointLimitAlgorithm restorePointLimitAlgorithm = new NumberAlgorithm(backupTask.Backup, 3);
        MergeAlgorithm mergeAlgorithm = new MergeAlgorithm();
        BackupTaskExtra backupTaskExtra =
            new BackupTaskExtra(backupTask, restorePointLimitAlgorithm, logger, mergeAlgorithm);
        backupTaskExtra.ReestablishRestorePoint("RestorePoint1");
        var newBackupTask = backupTaskExtra.Load("/Users/siriusa/Desktop/Repo5/json.txt");
        /*Assert.Equal(1, backupTaskExtra.GetBackup().GetRestorePoints().Count);*/
        Assert.Equal(newBackupTask.NameOfBackupTask, backupTask.NameOfBackupTask);
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
        var currentStorage = backupTask.RunTask();
        backupTask.RunTask();
        backupTask.RunTask();
        backupTask.RunTask();
        backupTask.RunTask();
        ILogger logger = new ConsoleLogger();
        IRestorePointLimitAlgorithm restorePointLimitAlgorithm = new NumberAlgorithm(backupTask.Backup, 3);
        MergeAlgorithm mergeAlgorithm = new MergeAlgorithm();
        BackupTaskExtra backupTaskExtra =
            new BackupTaskExtra(backupTask, restorePointLimitAlgorithm, logger, mergeAlgorithm);
        backupTaskExtra.CleanRestorePoints();
        Assert.Equal(3, backupTaskExtra.GetBackup().GetRestorePoints().Count);
    }
}