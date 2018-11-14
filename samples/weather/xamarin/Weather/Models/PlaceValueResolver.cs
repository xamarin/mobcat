using System;
using AutoMapper;
using Xamarin.Essentials;

namespace Weather.Models
{
    public class PlaceValueResolver : IValueResolver<Placemark, Place, string>
    {
        public string Resolve(Placemark source, Place destination, string destMember, ResolutionContext context)
        {
            var cityName = default(string);
            if (!string.IsNullOrEmpty(source.Locality))
            {
                cityName = source.Locality;
            }
            else
            {
                cityName = source.FeatureName;
            }
            return cityName;
        }
    }
}