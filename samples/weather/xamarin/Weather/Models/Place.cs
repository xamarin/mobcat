using System;
using Xamarin.Essentials;

namespace Weather.Models
{
    public class Place
    {
        public string CityName { get; set; }

        public Place()
        {
        }

        public Place(Placemark placemark)
        {
            if (!string.IsNullOrEmpty(placemark.Locality))
            {
                CityName = placemark.Locality;
            }
            else
            {
                CityName = placemark.FeatureName;
            }
        }
    }
}
