using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM.Abstractions;

namespace Microsoft.MobCAT.MVVM
{
    public class BaseShellViewModel : BaseNavigationViewModel
    {
        private IShellNavigationService _shellNavigationService;
        protected IShellNavigationService ShellNavigationService
        {
            get
            {
                if (_shellNavigationService == null)
                {
                    _shellNavigationService = Navigation as IShellNavigationService;
                }
                return _shellNavigationService;
            }
        }

        protected Task GoToRouteAsync(string route)
        {
            return ShellNavigationService?.GoToRouteAsync(route);
        }

        protected Task GoToRouteAsync(Uri route)
        {
            return ShellNavigationService?.GoToRouteAsync(route);
        }
    }
}