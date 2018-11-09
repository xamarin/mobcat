using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.MobCat;
using Microsoft.MobCat.MVVM;
using Weather.Services.Abstractions;
using Xamarin.Essentials;
using System.Linq;
using System.Threading;
using Weather.Models;

#if UNITTEST
using Geolocation = Weather.UnitTests.MockServices.Geolocation;
using Geocoding = Weather.UnitTests.MockServices.Geocoding;
#else
using Geolocation = Xamarin.Essentials.Geolocation;
using Geocoding = Xamarin.Essentials.Geocoding;
#endif


namespace Weather.ViewModels
{
    public class WeatherViewModel : BaseViewModel
    {
        string _cityName;
        string _weatherDescription;
        string _backgroundImage;
        string _currentTemp;
        string _highTemp;
        string _lowTemp;
        string _time;
        string _weatherIcon;
        bool _isCelsius;

        IForecastsService forecastsService;
        IImageService imageService;
        Timer _timer;

        public WeatherViewModel()
        {
            CityName = "London";
            IsCelsius = true;
            WeatherDescription = "Cloudy";
            CurrentTemp = "17";
            HighTemp = "20";
            LowTemp = "10";
            BackgroundImage = $"https://upload.wikimedia.org/wikipedia/commons/8/82/London_Big_Ben_Phone_box.jpg";

            // Timer to update time
            _timer = new Timer((state) => Time = DateTime.Now.ToShortTimeString(), null, 100, 10000);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _timer.Dispose();
        }


        public string CityName
        {
            get { return _cityName; }
            set
            {

                RaiseAndUpdate(ref _cityName, value);
            }
        }

        public string CurrentTemp
        {
            get { return _currentTemp; }
            set
            {
                RaiseAndUpdate(ref _currentTemp, value);
            }
        }

        public string HighTemp
        {
            get { return _highTemp; }
            set
            {
                RaiseAndUpdate(ref _highTemp, value);
            }
        }

        public string LowTemp
        {
            get { return _lowTemp; }
            set
            {
                RaiseAndUpdate(ref _lowTemp, value);
            }
        }

        public string WeatherDescription
        {
            get { return _weatherDescription; }
            set
            {
                RaiseAndUpdate(ref _weatherDescription, value);
            }
        }

        public string BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                RaiseAndUpdate(ref _backgroundImage, value);
            }
        }

        public bool IsCelsius
        {
            get { return _isCelsius; }
            set
            {
                RaiseAndUpdate(ref _isCelsius, value);
            }
        }

        public string TempSymbol
        {
            get { return IsCelsius ? "°C" : "°F"; }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                RaiseAndUpdate(ref _time, value);
            }
        }

        public string WeatherIcon
        {
            get { return _weatherIcon; }
            set
            {
                RaiseAndUpdate(ref _weatherIcon, value);
            }
        }



        public async override Task InitAsync()
        {
            forecastsService = ServiceContainer.Resolve<IForecastsService>();
            imageService = ServiceContainer.Resolve<IImageService>();

            try
            {
                // Use last known location for quicker response
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync();
                }

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    var place = await Geocoding.GetPlacemarksAsync(location);
                    string city = place.FirstOrDefault()?.Locality;
                    if (string.IsNullOrEmpty(city))
                    {
                       city = place.FirstOrDefault()?.FeatureName;
                    }
                    CityName = city;

                    var forecast = await forecastsService.GetForecastAsync(city);

                    if (forecast != null)
                    {
                        var londonCityWeatherImage = await imageService.GetImageAsync(forecast.Name, forecast.Overview);
                        Debug.WriteLine($"{forecast.Name}: {forecast.CurrentTemperature}, {forecast.Overview}");
                        Debug.WriteLine(londonCityWeatherImage);
                        WeatherDescription = forecast.Overview;
                        WeatherIcon = WeatherIcons.Lookup(WeatherDescription);
                        CurrentTemp = forecast.CurrentTemperature;
                        HighTemp = forecast.MaxTemperature;
                        LowTemp = forecast.MinTemperature;
                        BackgroundImage = await imageService.GetImageAsync(city, forecast.Overview);
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                CityName = "Unable to retrieve location - Feature not supported";
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                CityName = "Unable to retrieve location - Need permission";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Unable to get location
            }
        }

    }
}
