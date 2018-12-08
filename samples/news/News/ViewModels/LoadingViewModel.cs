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
        public Command ContinueCommand { get; } 

        public LoadingViewModel()
        {
            ContinueCommand = new Command(async () => await Navigation.PushAsync(new HomeViewModel()));
        }
    }
}