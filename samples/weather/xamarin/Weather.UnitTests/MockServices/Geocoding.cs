using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Weather.UnitTests.MockServices
{
    public static class Geocoding
    {
        internal static Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location)
        {
            return Task.FromResult<IEnumerable<Placemark>>(new List<Placemark> { new Placemark() });
        }
    }
}
