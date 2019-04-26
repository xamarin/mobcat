using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Services;

namespace Microsoft.MobCAT
{
    public class MainThread
    {
        private static IThreadService _registeredService;
        private static Func<IThreadService> _registeredServiceFunc;

        /// <summary>
        /// Gets the currently registered IThreadService.
        /// </summary>
        /// <value>The current service.</value>
        public static IThreadService RegisteredService
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IThreadService>(true);
                    if (_registeredService == null)
                    {
                        _registeredService = new StubThreadService();
                        Logger.Warn("No thread service was registered. Falling back to a stub implementation");
                    }
                }
                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the IThreadService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(IThreadService service)
        {
            ServiceContainer.Register<IThreadService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Lazily registers the IThreadService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(Func<IThreadService> service)
        {
            ServiceContainer.Register<IThreadService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Gets a value indicating whether the current thread context is the Main Thread.
        /// </summary>
        /// <value><c>true</c> if it is Main thread; otherwise, <c>false</c>.</value>
        public static bool IsMain => RegisteredService.IsMain;

        /// <summary>
        /// Queues the action to be executed on the Main Thread.
        /// </summary>
        /// <param name="action">Action to run on Main Thead.</param>
        public static void Run(Action action) => RegisteredService.RunOnMainThread(action);

        /// <summary>
        /// Queues the action to be executed on the Main Thread and awaits it's execution.
        /// </summary>
        /// <returns>Task that is completed when the action has completed.</returns>
        /// <param name="action">Action to run on Main Thread.</param>
        public static Task RunAsync(Action action) => RegisteredService.RunOnMainThreadAsync(action);
    }

    internal class StubThreadService : IThreadService
    {
        /// <inheritdoc />
        public bool IsMain => true;

        /// <inheritdoc />
        public void RunOnMainThread(Action action)
        {
            action.Invoke();
        }

        /// <inheritdoc />
        public Task RunOnMainThreadAsync(Action action)
        {
            action.Invoke();
            return Task.FromResult(true);
        }
    }
}
