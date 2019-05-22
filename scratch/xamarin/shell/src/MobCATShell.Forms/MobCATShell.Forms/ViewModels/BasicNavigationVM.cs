using System;
using Microsoft.MobCAT.MVVM;
using MobCATShell.Forms.ViewModels;

namespace MobCATShell.Forms.ViewModels
{
    public class BasicNavigationVM : BaseShellViewModel
    {
        public AsyncCommand NavigatePageCommand => new AsyncCommand(() =>
        {
            return Navigation.PushAsync(new DetailsPageVM
            {
                Title = $"Navigated here from {nameof(BasicNavigationVM)}",
                IsDismissButtonVisible = false
            });
        });

        public AsyncCommand NavigateModalCommand => new AsyncCommand(() =>
        {
            return Navigation.PushModalAsync(new DetailsPageVM
            {
                Title = $"Navigated here from {nameof(BasicNavigationVM)}",
                IsDismissButtonVisible = true
            });
        });
    }
}
