using System;
using System.Threading.Tasks;
using AutoMapper;
using Weather.Models;
using Weather.Services.Abstractions;
using Xamarin.Essentials;

namespace Weather.Services
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<Coordinates> GetLastKnownLocationAsync()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            return location == null ? null : Mapper.Map<Location, Coordinates>(location);
        }

        public async Task<Coordinates> GetLocationAsync()
        {
            var location = await Geolocation.GetLocationAsync();
            return location == null ? null : Mapper.Map<Location, Coordinates>(location);
        }
    }
}
