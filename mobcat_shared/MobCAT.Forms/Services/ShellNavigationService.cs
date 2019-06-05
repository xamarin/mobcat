using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.Forms.Services;
using Microsoft.MobCAT.MVVM.Abstractions;
using Xamarin.Forms;

namespace MobCAT.Forms.Services
{
    public class ShellNavigationService : NavigationService, IRouteNavigationService
    {
        public Task GoToRouteAsync(string route)
        {
            return Shell.Current.GoToAsync(new ShellNavigationState(route));
        }

        public Task GoToRouteAsync(Uri route)
        {
            return Shell.Current.GoToAsync(new ShellNavigationState(route));
        }
    }
}
