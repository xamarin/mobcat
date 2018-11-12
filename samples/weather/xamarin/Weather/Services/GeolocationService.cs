using System;
using System.Threading.Tasks;
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
            return location == null ? null : new Coordinates(location);
        }

        public async Task<Coordinates> GetLocationAsync()
        {
            var location = await Geolocation.GetLocationAsync();
            return location == null ? null : new Coordinates(location);
        }
    }
}
