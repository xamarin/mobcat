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
        public void SetLocationRedmond()
        {
            app.Device.SetLocation(latitude: 47.673988, longitude: -122.121513);
            new MainPage().SetLocationToRedmond();
        }

        [Test]
        public void Repl()
        {
            if (TestEnvironment.IsTestCloud)
            {
                Assert.Ignore("Local only");
            }

            app.Repl();
        }
    }
}
