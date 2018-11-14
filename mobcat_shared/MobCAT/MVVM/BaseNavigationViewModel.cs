using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM.Abstractions;

namespace Microsoft.MobCAT.MVVM
{
    public class BaseNavigationViewModel : BaseViewModel
    {
        private static INavigationService _registeredService;
        private static Func<INavigationService> _registeredServiceFunc;

        /// <summary>
        /// Gets or sets whether the ViewModel is modal.
        /// </summary>
        /// <value><c>true</c> if is modal; otherwise, <c>false</c>.</value>
        public bool IsModal { get; set; }

        protected INavigationService Navigation
        {
            get
            {
                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<INavigationService>(true);

                    if (_registeredService == null)
                        Logger.Warn($"No {nameof(INavigationService)} implementation has been registered.");
                }

                return _registeredService;
            }
        }

        /// <summary>
        /// Registers the INavigationService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(INavigationService service)
        {
            ServiceContainer.Register<INavigationService>(service);
            _registeredServiceFunc = null;
            _registeredService = service;
        }

        /// <summary>
        /// Lazily registers the INavigationService
        /// </summary>
        /// <param name="service">Service to register.</param>
        public static void RegisterService(Func<INavigationService> service)
        {
            ServiceContainer.Register<INavigationService>(service);
            _registeredService = null;
            _registeredServiceFunc = service;
        }

        /// <summary>
        /// Dismisses the ViewModel.
        /// </summary>
        /// <returns>The Task.</returns>
        public virtual Task Dismiss()
        {
            if (IsModal)
                return Navigation.PopModalAsync();

            return Navigation.PopAsync();
        }
    }
}