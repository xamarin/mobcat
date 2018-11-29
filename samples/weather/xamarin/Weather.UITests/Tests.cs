using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Weather.UITests.Pages;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Weather.UITests
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void AllowLocationRedmond()
        {
            app.Device.SetLocation(latitude: 47.673988, longitude: -122.121513);
            new MainPage().AllowLocation();
        }

        [Test]
        public void DisallowLocation()
        {
            new MainPage().DisallowLocation();
        }
    }
}
