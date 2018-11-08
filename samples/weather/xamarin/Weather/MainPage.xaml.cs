using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Weather.Services.Abstractions;
using Weather.ViewModels;
using Xamarin.Forms;

namespace Weather
{
    public partial class MainPage : ContentPage
    {
        IForecastsService forecastsService = ServiceContainer.Resolve<IForecastsService>();
        IImageService imageService = ServiceContainer.Resolve<IImageService>();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new WeatherViewModel();


            forecastsService = ServiceContainer.Resolve<IForecastsService>();
            imageService = ServiceContainer.Resolve<IImageService>();
        }

        // TEMPORARY: Interim Services test pending ViewModel implementation
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TestServices().ConfigureAwait(false);
        }

        private async Task TestServices()
        {
            var londonForecast = await forecastsService.GetForecastAsync("London");

            if (londonForecast != null)
            {
                var londonCityWeatherImage = await imageService.GetImageAsync(londonForecast.Name, londonForecast.Overview);
                Debug.WriteLine($"{londonForecast.Name}: {londonForecast.CurrentTemperature}, {londonForecast.Overview}");
                Debug.WriteLine(londonCityWeatherImage);
            }       
        }
    }
}