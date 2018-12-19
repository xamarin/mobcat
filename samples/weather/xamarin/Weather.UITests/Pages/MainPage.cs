using System;
using System.Threading;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace Weather.UITests.Pages
{
    public class MainPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(MainPage)),
            iOS = x => x.Marked(nameof(MainPage))
        };

        protected readonly Query CityNameLabel;
        protected readonly Query RedmondLabel;

        public MainPage()
        {
            CityNameLabel = x => x.Marked(nameof(CityNameLabel));
            RedmondLabel = x => x.Text("Redmond");

        }

        public MainPage SetLocationToRedmond()
        {
            app.Device.SetLocation(latitude: 47.6739, longitude: -122.1215);
            Thread.Sleep(TimeSpan.FromMinutes(5)); //Wait for location to be set on the phone
            app.WaitForElement(RedmondLabel, timeout: TimeSpan.FromMinutes(3), retryFrequency: TimeSpan.FromSeconds(30));
            app.Screenshot("Location set to Redmond and VM initialized");
            return this;
        }
    }
}