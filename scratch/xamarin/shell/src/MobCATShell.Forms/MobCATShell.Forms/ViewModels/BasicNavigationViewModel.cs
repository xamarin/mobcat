using System;
using Microsoft.MobCAT.MVVM;
using MobCATShell.Forms.ViewModels;

namespace MobCATShell.Forms.ViewModels
{
    public class BasicNavigationViewModel : BaseShellViewModel
    {
        public AsyncCommand NavigatePageCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new DetailsPageViewModel
            {
                Title = $"Navigated here from {nameof(BasicNavigationViewModel)}",
                IsDismissButtonVisible = false
            });
        });

        public AsyncCommand NavigateModalCommand => new AsyncCommand(() =>
        {
            return Navigation.PushModalAsync(new DetailsPageViewModel
            {
                Title = $"Navigated here from {nameof(BasicNavigationViewModel)}",
                IsDismissButtonVisible = true
            });
        });
    }
}
