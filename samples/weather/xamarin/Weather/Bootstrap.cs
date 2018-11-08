using System;
using Microsoft.MobCat;
using Weather.Services;
using Weather.Services.Abstractions;

namespace Weather
{
    public static class Bootstrap
    {
        public static void Begin(
            Action platformSpecificBegin)
        {
            var weatherServiceApiKey = Keys.WeatherServiceApiKey;

            if (string.IsNullOrWhiteSpace(weatherServiceApiKey))
                throw new Exception("No API key set. Pending implementation of secrets handling mechanism, set this manually in Bootstrap.cs line 14");

            ServiceContainer.Register<IForecastsService>(() => new ForecastsService(weatherServiceApiKey));
            ServiceContainer.Register<IImageService>(() => new ImageService(weatherServiceApiKey));
           
            platformSpecificBegin();
        }
    }
}