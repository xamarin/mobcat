using System.IO;
using Weather.Models;

namespace Weather.Services.Abstractions
{
    public interface IFileStorageService
    {
        bool FileExists(string path);

        bool DeleteFile(string path);

        long FileSize(string path);

        Stream CreateOutputStream(string path, OutputStreamMode mode = OutputStreamMode.OverwriteOrCreate);

        Stream CreateInputStream(string path);

        string BasePath { get; }
    }
}