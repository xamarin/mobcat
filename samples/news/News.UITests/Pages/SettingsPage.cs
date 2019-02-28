using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace News.UITests.Pages
{
    public class SettingsPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(SettingsPage)),
            iOS = x => x.Marked(nameof(SettingsPage))
        };

        protected readonly Query LightModeSwitch;

        public SettingsPage()
        {
            LightModeSwitch = x => x.Marked(nameof(LightModeSwitch));
        }

        public void SetLightMode()
        {
            app.WaitForElement(LightModeSwitch);
            app.Tap(LightModeSwitch);
            app.Screenshot("Toggled light mode");
        }
    }
}
