using System;
using System.IO;
using Microsoft.MobCAT;
using Microsoft.MobCAT.Services;

namespace MobCAT.Forms.Services
{
    public class FileSystemService : IFileSystemService
    {
        readonly string _documentStorage;
        readonly string _settingsStorage;
        readonly string _tempStorage;
    
        public FileSystemService()
        {
            var applicationSupportPath = Xamarin.Essentials.FileSystem.AppDataDirectory;

            Logger.Debug($"Application Support Path: {applicationSupportPath}");

            _documentStorage = Path.Combine(applicationSupportPath, "Documents");
            _settingsStorage = Path.Combine(applicationSupportPath, "Settings");
            _tempStorage = Xamarin.Essentials.FileSystem.CacheDirectory;

            if (!Directory.Exists(_documentStorage))
                Directory.CreateDirectory(_documentStorage);

            if (!Directory.Exists(_settingsStorage))
                Directory.CreateDirectory(_settingsStorage);

            if (!Directory.Exists(_tempStorage))
                Directory.CreateDirectory(_tempStorage);
        }


        /// <inheritdoc />
        public string DocumentStorage => _documentStorage;

        /// <inheritdoc />
        public string SettingsStorage => _settingsStorage;

        /// <inheritdoc />
        public string TempStorage => _tempStorage;
    }
}
