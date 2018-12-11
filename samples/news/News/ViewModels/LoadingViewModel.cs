using System;
using System.Threading.Tasks;
using Microsoft.MobCAT.MVVM;
using News;

namespace News.ViewModels
{
    /// <summary>
    /// Loading view model.
    /// </summary>
    public class LoadingViewModel : BaseNavigationViewModel
    {
        public AsyncCommand ContinueCommand { get; }

        public LoadingViewModel()
        {
            ContinueCommand = new AsyncCommand(OnContinue);
        }

        private async Task OnContinue()
        {
            await Navigation.PushAsync(new HomeViewModel(), true);
        }
    }
}