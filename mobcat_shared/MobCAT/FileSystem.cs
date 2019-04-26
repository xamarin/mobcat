using System;
using System.IO;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT
{
    public static class FileSystem
    {
        private static IFileSystemService _registeredService;
        private static Func<IFileSystemService> _registeredServiceFunc;

        /// <summary>
        /// Gets the currently registered IFileSystemService.
        /// </summary>
        /// <value>The current service.</value>
        public static IFileSystemService CurrentService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IFileSystemService>(false);
                }

                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the IFileSystemService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(IFileSystemService service)
        {
            ServiceContainer.Register<IFileSystemService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Lazily registers the IFileSystemService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(Func<IFileSystemService> service)
        {
            ServiceContainer.Register<IFileSystemService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Gets the document storage folder path.
        /// </summary>
        /// <value>The document storage path.</value>
        public static string DocumentStorage => CurrentService.DocumentStorage;

        /// <summary>
        /// Gets the settings storage.
        /// </summary>
        /// <value>The settings storage path.</value>
        public static string SettingsStorage => CurrentService.SettingsStorage;

        /// <summary>
        /// Gets the temp storage path.
        /// </summary>
        /// <value>The temp storage path.</value>
        public static string TempStorage => CurrentService.TempStorage;
    }
}
