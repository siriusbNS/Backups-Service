namespace Backups.Tools;

internal class ListOfBackupObjEmptyException : Exception
{
    public ListOfBackupObjEmptyException(string message)
        : base(message) { }
}