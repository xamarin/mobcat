using System;
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

        protected readonly Query AllowLocationButton;
        protected readonly Query DontAllowLocationButton;
        protected readonly Query CityNameLabel;

        public MainPage()
        {
            AllowLocationButton = x => x.Marked("Allow");
            DontAllowLocationButton = x => x.Marked("Don't Allow");
            CityNameLabel = x => x.Marked(nameof(CityNameLabel));

        }

        public MainPage AllowLocation()
        {
            app.WaitForElement(AllowLocationButton);
            app.Screenshot("Tapping on allow location button");
            app.Tap(AllowLocationButton);
            app.WaitForElement(CityNameLabel);
            app.Screenshot("VM Initialized");
            return this;
        }

        public MainPage DisallowLocation()
        {
            app.WaitForElement(DontAllowLocationButton);
            app.Screenshot("Tapping on don't allow location button");
            app.Tap(DontAllowLocationButton);
            app.WaitForElement(CityNameLabel);
            app.Screenshot("VM Initialized");
            return this;
        }
    }
}