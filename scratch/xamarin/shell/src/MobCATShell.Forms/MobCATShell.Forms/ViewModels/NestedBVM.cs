using System;
using Microsoft.MobCAT;
using Microsoft.MobCAT.MVVM;
using MobCATShell.Forms.Services;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms.ViewModels
{
    public class NestedBVM : BaseShellViewModel
    {
        private IRouteService _routeService;
        private IRouteService RouteService => _routeService ?? (_routeService = ServiceContainer.Resolve<IRouteService>());

        public AsyncCommand DismissCommand => new AsyncCommand(Dismiss);

        public AsyncCommand NavigateToDetailsCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new DetailsPageVM
            {
                Title = $"Navigated from {nameof(NestedBVM)}"
            });
        });

        public AsyncCommand RouteToADetailsCommand => new AsyncCommand(() =>
        {
            var route = RouteService.BDetailsRoute;
            var query = $"{nameof(DetailsPage.RoutedTitle)}=Routed using {nameof(RouteService.BDetailsRoute)}" +
            $"&{nameof(DetailsPage.RoutedDismissButtonVisibility)}={true.ToString()}";
            return GoToRouteAsync($"{route}?{query}");
        });
    }
}
