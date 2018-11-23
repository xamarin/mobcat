using System;
using System.Reflection;
using Microsoft.MobCAT;
using Microsoft.MobCAT.Forms.Services;
using Microsoft.MobCAT.MVVM.Abstractions;
using Weather.Models;
using Weather.Services;
using Weather.Services.Abstractions;
using Xamarin.Essentials;

namespace Communicator
{
    public static class Bootstrap
    {
        public static void Begin(Action platformSpecificBegin = null)
        {
            var navigationService = new NavigationService();
            navigationService.RegisterViewModels(typeof(MainPage).GetTypeInfo().Assembly);

            ServiceContainer.Register<INavigationService>(navigationService);

            platformSpecificBegin?.Invoke();
        }
    }
}