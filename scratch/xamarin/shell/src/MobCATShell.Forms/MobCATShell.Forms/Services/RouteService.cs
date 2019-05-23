using System;
using System.Linq;
using System.Collections.Generic;
using MobCATShell.Forms.ViewModels;
using Xamarin.Forms;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms.Services
{
    public class RouteService : IRouteService
    {
        private readonly Random _random;
        private readonly List<string> _routes;

        public string BasicNavTabRoute { get; private set; }
        public string NestedNavTabRoute { get; private set; }
        public string ShellDetailsRoute { get; private set; }

        public string NestedARoute { get; private set; }
        public string NestedBRoute { get; private set; }

        public string ADetailsRoute { get; private set; }
        public string BDetailsRoute { get; private set; }

        public RouteService()
        {
            _random = new Random();

            BasicNavTabRoute = $"//{nameof(BasicNavigationViewModel)}";
            NestedNavTabRoute = $"//{nameof(NestedNavigationViewModel)}";
            ShellDetailsRoute = $"//{nameof(DetailsPageViewModel)}";

            NestedARoute = $"{nameof(NestedNavigationViewModel)}/{nameof(NestedAViewModel)}";
            Routing.RegisterRoute(NestedARoute, typeof(NestedAPage));

            NestedBRoute = $"{nameof(NestedNavigationViewModel)}/{nameof(NestedBViewModel)}";
            Routing.RegisterRoute(NestedBRoute, typeof(NestedBPage));

            ADetailsRoute = $"{nameof(NestedAViewModel)}/{nameof(DetailsPageViewModel)}";
            Routing.RegisterRoute(ADetailsRoute, typeof(DetailsPage));

            BDetailsRoute = $"{nameof(NestedBViewModel)}/{nameof(DetailsPageViewModel)}";
            Routing.RegisterRoute(BDetailsRoute, typeof(DetailsPage));

            _routes = new List<string>
            {
                BasicNavTabRoute,
                NestedNavTabRoute,
                NestedARoute,
                NestedBRoute,
                ADetailsRoute,
                BDetailsRoute,
                ShellDetailsRoute
            };
        }

        public string GetRandomRoute() => _routes[_random.Next(_routes.Count)];
    }
}
