using System.IO.Compression;
using Backups.Interfaces;
using Newtonsoft.Json;
using Zio;
using Zio.FileSystems;

namespace Backups.Entities;

public class RepoPhysicalFs : IRepository, IDisposable
{
    private IFileSystem _fileSystem;
    [JsonProperty("path")]
    private string _path;
    public RepoPhysicalFs(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        _fileSystem = new PhysicalFileSystem();
        _path = path;
        if (!Directory.Exists(path))
            this.CreateDirectory(_path);
    }

    [JsonIgnore]
    public UPath Path => _path;
    public UPath GetPath()
    {
        return Path;
    }

    public Stream OpenFile(string nameOfFile)
    {
        var pathToFile = Path / nameOfFile;
        ArgumentNullException.ThrowIfNull(pathToFile);
        FileMode fileMode = FileMode.Create;
        if (File.Exists(pathToFile.ToString()))
            fileMode = FileMode.Open;
        return _fileSystem.OpenFile(pathToFile, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
    }

    public void CopyFile(UPath pathWhereIsFrom, UPath pathWhereIsTo)
    {
        ArgumentNullException.ThrowIfNull(pathWhereIsFrom);
        ArgumentNullException.ThrowIfNull(pathWhereIsTo);
        _fileSystem.CopyFile(pathWhereIsFrom, pathWhereIsTo, false);
    }

    public void CreateDirectory(UPath path)
    {
        ArgumentNullException.ThrowIfNull(path);
        _fileSystem.CreateDirectory(path);
    }

    public void DeleteDirectory(UPath path)
    {
        ArgumentNullException.ThrowIfNull(path);
        _fileSystem.DeleteDirectory(path, true);
    }

    public void DeleteFile(UPath path)
    {
        ArgumentNullException.ThrowIfNull(path);
        _fileSystem.DeleteFile(path);
    }

    public void Dispose()
    {
        _fileSystem.Dispose();
    }

    public void ArchiveFileInZip(string namesOfBackupObj, string pathToZip)
    {
        var fs = File.OpenRead((Path / namesOfBackupObj).ToString());
        var memFile = new MemoryStream();
        fs.CopyTo(memFile);
        memFile.Seek(0, SeekOrigin.Begin);
        var archiveStream = new MemoryStream();
        var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true);
        var zipArchiveEntry = archive.CreateEntry(namesOfBackupObj, CompressionLevel.Fastest);
        var zipStream = zipArchiveEntry.Open();
        zipStream.Write(memFile.ToArray(), 0, memFile.ToArray().Length);
        var fw = File.OpenWrite(pathToZip);
        var memZip = new MemoryStream(archiveStream.ToArray());
        memZip.CopyTo(fw);
        fw.Close();
    }
}