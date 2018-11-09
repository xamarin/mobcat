using System;
using System.IO;
using Weather.Models;
using Weather.Services.Abstractions;
using Xamarin.Essentials;

namespace Weather.Services
{
    public class EssentialsFileStorageService : IFileStorageService
    {
        public string BasePath => FileSystem.CacheDirectory;

        public bool FileExists(string path)
        {
            return File.Exists(GetAbsolutePath(path));
        }

        public bool DeleteFile(string path)
        {
            File.Delete(GetAbsolutePath(path));
            return true;
        }

        public long FileSize(string path)
        {
            path = GetAbsolutePath(path);
            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                return file.Length;
            }

            return 0;
        }

        public Stream CreateInputStream(string path)
        {
            return new FileStream(GetAbsolutePath(path), FileMode.Open, FileAccess.Read);
        }

        public Stream CreateOutputStream(string path, OutputStreamMode mode = OutputStreamMode.OverwriteOrCreate)
        {
            var absolutePath = GetAbsolutePath(path);

            FileMode fileMode = FileMode.OpenOrCreate;

            switch (mode)
            {
                case OutputStreamMode.Append:
                    fileMode = FileMode.Append;
                    break;
                case OutputStreamMode.OverwriteOrCreate:
                    fileMode = FileMode.Create;
                    break;
                case OutputStreamMode.OpenOrCreate:
                    fileMode = FileMode.OpenOrCreate;
                    break;
            }

            return new FileStream(GetAbsolutePath(path), fileMode, FileAccess.Write);
        }

        private string GetAbsolutePath(string path)
        {
            if (!path.StartsWith(BasePath, StringComparison.Ordinal))
                return Path.Combine(BasePath, path);

            return path;
        }
    }
}