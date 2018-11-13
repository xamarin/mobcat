using System;
using Xunit;
using AutoMapper;
using Xamarin.Essentials;
using Weather.Models;

namespace Weather.UnitTests
{
    public class MappingTests : IDisposable
    {
        public MappingTests()
        {
            Bootstrap.Begin();
        }

        public void Dispose()
        {
            Mapper.Reset();
        }

        [Fact]
        public void TestAutomapperConfig()
        {
            var exception = Record.Exception(() => Mapper.Configuration.AssertConfigurationIsValid()); //Testing Automapper config: https://automapper.readthedocs.io/en/latest/Getting-started.html
            Assert.Null(exception); //Assert there's no exception
        }

        [Fact]
        public void TestCityNameFromLocality()
        {
            var placemark = new Placemark
            {
                Locality = nameof(Placemark.Locality),
                FeatureName = nameof(Placemark.FeatureName)
            };
            var place = Mapper.Map<Placemark, Place>(placemark);
            Assert.Equal(placemark.Locality, place.CityName);
            Assert.NotEqual(placemark.FeatureName, place.CityName);
        }

        [Fact]
        public void TestCityNameFromFeatureName()
        {
            var placemark = new Placemark
            {
                Locality = default(string),
                FeatureName = nameof(Placemark.FeatureName)
            };
            var place = Mapper.Map<Placemark, Place>(placemark);
            Assert.Equal(placemark.FeatureName, place.CityName);
            Assert.NotEqual(placemark.Locality, place.CityName);
        }

        [Fact]
        public void TestLocationToCoordinatesMapping()
        {
            var latitude = 1.1;
            var longitude = 2.2;
            var location = new Location(latitude, longitude);
            var coordinates = Mapper.Map<Location, Coordinates>(location);
            Assert.Equal(coordinates.Latitude, location.Latitude);
            Assert.Equal(coordinates.Longitude, location.Longitude);
        }
    }
}
