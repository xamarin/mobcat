using System;
using Xamarin.Essentials;

namespace Weather.Models
{
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates()
        {
        }

        public Coordinates(Location location)
        {
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }
}
