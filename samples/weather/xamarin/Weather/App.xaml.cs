using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Weather.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Weather
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage { ViewModel = new WeatherViewModel() };
        }

        protected override void OnStart()
        {
            try
            {
                // Handle when your app starts
                AppCenter.Start($"{ServiceConfig.ANDROIDAPPCENTERSECRET};" +
                      $"{ServiceConfig.IOSAPPCENTERSECRET}",
                      typeof(Analytics), typeof(Crashes));
            }
            catch (System.Exception ex)
            {
                 //TODO: Log
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}