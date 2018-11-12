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
            ServiceContainer.Register<IForecastsService>(() => new ForecastsService(ServiceConfig.WeatherServiceUrl, ServiceConfig.WeatherServiceApiKey));
            ServiceContainer.Register<IImageService>(() => new ImageService(ServiceConfig.WeatherServiceUrl, ServiceConfig.WeatherServiceApiKey));

            platformSpecificBegin();
        }
    }
}