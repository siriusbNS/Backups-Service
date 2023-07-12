namespace Backups.Tools;

internal class RestorePointException : Exception
{
    public RestorePointException(string message)
        : base(message) { }
}