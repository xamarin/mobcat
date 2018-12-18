using System;
using System.Threading.Tasks;
using Microsoft.MobCAT;
using Microsoft.MobCAT.MVVM;
using Weather.Services.Abstractions;
using System.Linq;
using System.Threading;
using Weather.Models;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Weather.ViewModels
{
    public class WeatherViewModel : BaseNavigationViewModel
    {
        string _cityName;
        string _weatherDescription;
        string _weatherImage;
        string _currentTemp;
        string _highTemp;
        string _lowTemp;
        string _time;
        string _weatherIcon;
        bool _isCelsius;

        IForecastsService forecastsService;
        IImageService imageService;
        IGeolocationService geolocationService;
        IGeocodingService geocodingService;
        IValueCacheService valueCacheService;
        ILocalizationService localizationService;

        readonly Lazy<ITimeOfDayImageService> timeOfDayImageService = new Lazy<ITimeOfDayImageService>(() =>
        {
            try
            {
                return ServiceContainer.Resolve<ITimeOfDayImageService>();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        });

        Timer _timer;

        public WeatherViewModel()
        {
            IsCelsius = true;

            if (DesignMode.IsDesignModeEnabled)
            {
                CityName = "London";
                IsCelsius = true;
                WeatherDescription = "Cloudy";
                CurrentTemp = "17";
                HighTemp = "20";
                LowTemp = "10";
                WeatherImage = $"https://upload.wikimedia.org/wikipedia/commons/8/82/London_Big_Ben_Phone_box.jpg";
            }

            // Timer to update time
            _timer = new Timer((state) => Time = DateTime.Now.ToShortTimeString(), state: null, dueTime: 100, period: 10000);
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

        public string BackgroundImage => timeOfDayImageService.Value?.GetImageForDateTime(DateTime.Now);

        public string WeatherImage
        {
            get => _weatherImage;
            set => RaiseAndUpdate(ref _weatherImage, value);
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
            geolocationService = ServiceContainer.Resolve<IGeolocationService>();
            geocodingService = ServiceContainer.Resolve<IGeocodingService>();
            valueCacheService = ServiceContainer.Resolve<IValueCacheService>();
            localizationService = ServiceContainer.Resolve<ILocalizationService>();

            LoadWeatherState(); //load the saved weather state first

            try
            {
                // Use last known location for quicker response
                var location = await geolocationService.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await geolocationService.GetLocationAsync();
                }

                if (location != null)
                {
                    var places = await geocodingService.GetPlacesAsync(location);
                    CityName = places.FirstOrDefault()?.CityName;

                    var forecast = await forecastsService.GetForecastAsync(location.Latitude, location.Longitude); //Use lat and long since the city name can be different based on localization

                    if (forecast != null)
                    {
                        var weatherDescription = forecast.Overview;
                        if (!string.IsNullOrEmpty(weatherDescription))
                        {
                            WeatherIcon = WeatherIcons.Lookup(weatherDescription);
                            WeatherDescription = localizationService.Translate(weatherDescription.Trim().Replace(" ", "").ToLower());
                        }
                        else
                        {
                            WeatherDescription = localizationService.Translate(Constants.LanguageResourceKeys.WeatherUnknownKey);
                        }
                        CurrentTemp = forecast.CurrentTemperature;
                        HighTemp = forecast.MaxTemperature;
                        LowTemp = forecast.MinTemperature;
                        WeatherImage = await imageService.GetImageAsync(forecast.Name, forecast.Overview);
                    }
                }

                SaveWeatherState();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                CityName = "Unable to retrieve location - Feature not supported";
                Logger.Error(fnsEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                CityName = "Unable to retrieve location - Need permission";
                Logger.Error(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Logger.Error(ex);
            }
            finally
            {
                //Use cached weather image as fallback if necessary
                if (string.IsNullOrEmpty(WeatherImage))
                {
                    WeatherImage = valueCacheService.Load(Constants.CacheKeys.WeatherImageKey, default(string));
                }
            }
        }

        private void SaveWeatherState()
        {
            valueCacheService.Save(Constants.CacheKeys.CacheSavedDateTimeKey, DateTime.Now);
            valueCacheService.Save(Constants.CacheKeys.CityNameKey, CityName);
            valueCacheService.Save(Constants.CacheKeys.CurrentTempKey, CurrentTemp);
            valueCacheService.Save(Constants.CacheKeys.HighTempKey, HighTemp);
            valueCacheService.Save(Constants.CacheKeys.LowTempKey, LowTemp);
            valueCacheService.Save(Constants.CacheKeys.WeatherDescriptionKey, WeatherDescription);
            valueCacheService.Save(Constants.CacheKeys.WeatherImageKey, WeatherImage);
            valueCacheService.Save(Constants.CacheKeys.WeatherIconKey, WeatherIcon);
            valueCacheService.Save(Constants.CacheKeys.IsCelsiusKey, IsCelsius);
        }

        private void LoadWeatherState()
        {
            var cacheSavedDateTime = valueCacheService.Load(Constants.CacheKeys.CacheSavedDateTimeKey, default(DateTime));
            if ((DateTime.Now - cacheSavedDateTime).TotalDays < 1) //Only load if it's been less than a day
            {
                CityName = valueCacheService.Load(Constants.CacheKeys.CityNameKey, default(string));
                CurrentTemp = valueCacheService.Load(Constants.CacheKeys.CurrentTempKey, default(string));
                HighTemp = valueCacheService.Load(Constants.CacheKeys.HighTempKey, default(string));
                LowTemp = valueCacheService.Load(Constants.CacheKeys.LowTempKey, default(string));
                WeatherDescription = valueCacheService.Load(Constants.CacheKeys.WeatherDescriptionKey, default(string));
                WeatherIcon = valueCacheService.Load(Constants.CacheKeys.WeatherIconKey, default(string));
                IsCelsius = valueCacheService.Load(Constants.CacheKeys.IsCelsiusKey, default(bool));
            }
        }
    }
}
