using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.MobCAT;
using Weather.Models;
using Weather.Services.Abstractions;
using Xamarin.Essentials;

namespace Weather.Services
{
    public class GeolocationService : IGeolocationService
    {
        readonly Lazy<IMainThreadAsyncService> mainThreadAsyncService = new Lazy<IMainThreadAsyncService>(() => ServiceContainer.Resolve<IMainThreadAsyncService>());

        public async Task<Coordinates> GetLastKnownLocationAsync()
        {
            var location = await mainThreadAsyncService.Value.RunOnMainThreadAsync(Geolocation.GetLastKnownLocationAsync);
            return location == null ? null : Mapper.Map<Location, Coordinates>(location);
        }

        public async Task<Coordinates> GetLocationAsync()
        {
            var location = await mainThreadAsyncService.Value.RunOnMainThreadAsync(Geolocation.GetLocationAsync);
            return location == null ? null : Mapper.Map<Location, Coordinates>(location);
        }
    }
}
