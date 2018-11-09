using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Weather.UnitTests.MockServices
{
    public static class Geolocation
    {
        internal static Task<Location> GetLastKnownLocationAsync()
        {
            return Task.FromResult(new Location());
        }

        internal static Task<Location> GetLocationAsync(GeolocationRequest request = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new Location());
        }
    }
}
