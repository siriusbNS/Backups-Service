using Zio;

namespace Backups.Interfaces;

public interface IRepository
{
    UPath GetPath();
    Stream OpenFile(string nameOfFile);
    void CopyFile(UPath pathWhereIsFrom, UPath pathWhereIsTo);
    void CreateDirectory(UPath path);
    void DeleteDirectory(UPath path);
    void DeleteFile(UPath path);
    void ArchiveFileInZip(string namesOfBackupObj, string pathToZip);
}