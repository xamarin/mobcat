using System;
using Microsoft.MobCAT;
using Microsoft.MobCAT.MVVM;
using MobCATShell.Forms.Services;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms.ViewModels
{
    public class NestedNavigationViewModel : BaseShellViewModel
    {
        private IRouteService _routeService;
        private IRouteService RouteService => _routeService ?? (_routeService = ServiceContainer.Resolve<IRouteService>());

        public AsyncCommand RandomRouteCommand => new AsyncCommand(() =>
        {
            var route = RouteService.GetRandomRoute();
            return GoToRouteAsync(route);
        });

        public AsyncCommand NavigateToACommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new NestedAViewModel());
        });

        public AsyncCommand NavigateToBCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new NestedBViewModel());
        });

        public AsyncCommand NavigateToDetailsCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new DetailsPageViewModel
            {
                Title = $"Navigated from {nameof(NestedNavigationViewModel)}"
            });
        });

        public AsyncCommand RouteToACommand => new AsyncCommand(() =>
        {
            return GoToRouteAsync(RouteService.NestedARoute);
        });

        public AsyncCommand RouteToBCommand => new AsyncCommand(() =>
        {
            return GoToRouteAsync(RouteService.NestedBRoute);
        });

        public AsyncCommand RouteToADetailsCommand => new AsyncCommand(() =>
        {
            var route = RouteService.ADetailsRoute;
            var query = $"{nameof(DetailsPage.RoutedTitle)}=Routed using {nameof(RouteService.ADetailsRoute)}" +
            $"&{nameof(DetailsPage.RoutedDismissButtonVisibility)}={true.ToString()}";
            return GoToRouteAsync($"{route}?{query}");
        });

        public AsyncCommand RouteToBDetailsCommand => new AsyncCommand(() =>
        {
            var route = RouteService.ADetailsRoute;
            var query = $"{nameof(DetailsPage.RoutedTitle)}=Routed using {nameof(RouteService.BDetailsRoute)}" +
            $"&{nameof(DetailsPage.RoutedDismissButtonVisibility)}={true.ToString()}";
            return GoToRouteAsync($"{route}?{query}");
        });

        public AsyncCommand RouteToBasicNavTabCommand => new AsyncCommand(() =>
        {
            var route = RouteService.BasicNavTabRoute;
            return GoToRouteAsync(route);
        });
    }
}
