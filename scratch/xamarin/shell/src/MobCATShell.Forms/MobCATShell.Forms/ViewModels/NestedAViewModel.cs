using System;
using Microsoft.MobCAT;
using Microsoft.MobCAT.MVVM;
using MobCATShell.Forms.Services;
using MobCATShell.Forms.Views;

namespace MobCATShell.Forms.ViewModels
{
    public class NestedAViewModel : BaseShellViewModel
    {
        private IRouteService _routeService;
        private IRouteService RouteService => _routeService ?? (_routeService = ServiceContainer.Resolve<IRouteService>());

        public AsyncCommand DismissCommand => new AsyncCommand(Dismiss);

        public AsyncCommand NavigateToDetailsCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new DetailsPageViewModel
            {
                Title = $"Navigated from {nameof(NestedAViewModel)}"
            });
        });

        public AsyncCommand RouteToADetailsCommand => new AsyncCommand(() =>
        {
            var route = RouteService.ADetailsRoute;
            var query = $"{nameof(DetailsPage.RoutedTitle)}=Routed using {nameof(RouteService.ADetailsRoute)}" +
            $"&{nameof(DetailsPage.RoutedDismissButtonVisibility)}={true.ToString()}";
            return Navigation.GoToRouteAsync($"{route}?{query}");
        });
    }
}
