using System;
using AutoMapper;
using Microsoft.MobCat;
using Weather.Models;
using Weather.Services;
using Weather.Services.Abstractions;
using Xamarin.Essentials;

namespace Weather
{
    public static class Bootstrap
    {
        public static void Begin(
            Action platformSpecificBegin = null)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Location, Coordinates>();
                cfg.CreateMap<Placemark, Place>()
                .ForMember(dest => dest.CityName, opt => opt.ResolveUsing<PlaceValueResolver>());
            });

            ServiceContainer.Register<IForecastsService>(() => new ForecastsService(ServiceConfig.WeatherServiceUrl, ServiceConfig.WeatherServiceApiKey));
            ServiceContainer.Register<IImageService>(() => new ImageService(ServiceConfig.WeatherServiceUrl, ServiceConfig.WeatherServiceApiKey));
            ServiceContainer.Register<IGeolocationService>(() => new GeolocationService());
            ServiceContainer.Register<IGeocodingService>(() => new GeocodingService());

            platformSpecificBegin?.Invoke();
        }
    }
}