using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Microsoft.MobCat.MVVM;
using Weather.Services.Abstractions;

namespace Weather.ViewModels
{
    public class WeatherViewModel : BaseViewModel
    {
        string _cityName;

        IForecastsService forecastsService = ServiceContainer.Resolve<IForecastsService>();
        IImageService imageService = ServiceContainer.Resolve<IImageService>();
 
        public WeatherViewModel()
        {
            CityName = "London";

            forecastsService = ServiceContainer.Resolve<IForecastsService>();
            imageService = ServiceContainer.Resolve<IImageService>();
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {

                RaiseAndUpdate(ref _cityName, value);
            }
        }

        public string Time
        {
            get { return DateTime.Now.ToShortTimeString(); }
        }

        public override Task InitAsync()
        {
            return TestServices();
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
