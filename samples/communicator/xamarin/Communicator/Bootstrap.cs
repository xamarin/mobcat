using System;
using System.Reflection;
using Microsoft.MobCAT;
using Microsoft.MobCAT.Forms.Services;
using Microsoft.MobCAT.MVVM.Abstractions;
using Xamarin.Essentials;

namespace Communicator
{
    public static class Bootstrap
    {
        public static void Begin(Action platformSpecificBegin = null)
        {
            var navigationService = new NavigationService();
            navigationService.RegisterViewModels(typeof(MainPage).GetTypeInfo().Assembly);
            navigationService.RegisterViewModels(typeof(LoginPage).GetTypeInfo().Assembly);

            ServiceContainer.Register<INavigationService>(navigationService);

            platformSpecificBegin?.Invoke();
        }
    }
}