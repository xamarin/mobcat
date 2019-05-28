using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM.Abstractions;

namespace Microsoft.MobCAT.MVVM
{
    public class BaseShellViewModel : BaseNavigationViewModel
    {
        private IRoutelNavigationService _registeredService;
        private static Func<IRoutelNavigationService> _registeredServiceFunc;
        protected new IRoutelNavigationService Navigation
        {
            get
            {

                if (_registeredService == null)
                {
                    _registeredService = _registeredServiceFunc?.Invoke() ?? ServiceContainer.Resolve<IRoutelNavigationService>(true);

                    if (_registeredService == null)
                        Logger.Warn($"No {nameof(IRoutelNavigationService)} implementation has been registered.");
                }

                return _registeredService;
            }
        }
    }
}