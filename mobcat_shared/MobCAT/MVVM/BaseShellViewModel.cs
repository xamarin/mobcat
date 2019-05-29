using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM.Abstractions;

namespace Microsoft.MobCAT.MVVM
{
    public class BaseShellViewModel : BaseNavigationViewModel
    {
        private IRouteNavigationService _registeredService;
        private static Func<IRouteNavigationService> _registeredServiceFunc;
        protected new IRouteNavigationService Navigation
        {
            get
            {

                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IRouteNavigationService>(true);

                    if (_registeredService == null)
                        Logger.Warn($"No {nameof(IRouteNavigationService)} implementation has been registered.");
                }

                return _registeredService;
            }
        }
    }
}