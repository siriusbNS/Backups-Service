
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