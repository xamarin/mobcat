using System;
using Microsoft.MobCAT.Services;
using MobCAT;

namespace Microsoft.MobCAT
{
    public static class Progress
    {
        private static IProgressService _registeredService;
        private static Func<IProgressService> _registeredServiceFunc;

        public static IProgressService RegisteredService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IProgressService>(true);
                    if (_registeredService == null)
                    {
                        _registeredService = new ConsoleProgressService();
                        Logger.Warn("No progress service was registered.  Falling back to console progress.");
                    }
                }
                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the IProgressService instance.
        /// </summary>
        /// <param name="service">IProgressService implementation.</param>
        public static void RegisterService(IProgressService service)
        {
            ServiceContainer.Register<IProgressService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Registers the IProgressService instance.
        /// </summary>
        /// <param name="service">IProgressService implementation.</param>
        public static void RegisterService(Func<IProgressService> service)
        {
            ServiceContainer.Register<IProgressService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Displays the progress.
        /// </summary>
        /// <param name="title">Title.</param>
        public static void DisplayProgress(string title = null) => RegisteredService.DisplayProgress(title);

        /// <summary>
        /// Hides the progress.
        /// </summary>
        public static void HideProgress() => RegisteredService.HideProgress();
    }
}
