using System;
using Microsoft.MobCAT;
using Microsoft.MobCAT.Forms.Services;
using Microsoft.MobCAT.MVVM.Abstractions;
using MobCAT.Forms.Services;
using MobCATShell.Forms.Services;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms
{
    public static class Bootstrap
    {
        public static void Begin(Action platformSpecificBegin = null)
        {
            var shellNavigationService = new ShellNavigationService();
            shellNavigationService.RegisterViewModels(typeof(BasicNavigationPage).Assembly);
            ServiceContainer.Register<INavigationService>(shellNavigationService);
            ServiceContainer.Register<IRouteService>(new RouteService());
            platformSpecificBegin?.Invoke();
        }
    }
}
