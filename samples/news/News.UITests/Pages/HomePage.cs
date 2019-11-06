using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace News.UITests.Pages
{
    public class HomePage : BasePage
    {
        //Use the name of NewsPage since HomePage is not an actual Page
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(NewsPage)),
            iOS = x => x.Marked(nameof(NewsPage))
        };

        protected readonly Query NewsTab;
        protected readonly Query SourcesTab;
        protected readonly Query FavoritesTab;
        protected readonly Query SearchTab;
        protected readonly Query SettingsTab;

        public HomePage()
        {
            NewsTab = x => x.Marked("news");
            SourcesTab = x => x.Marked("sources");
            FavoritesTab = x => x.Marked("favorites");
            SearchTab = x => x.Marked("search");
            SettingsTab = x => x.Marked("settings");
        }

        public NewsPage ShowNewsTab()
        {
            app.WaitForElement(NewsTab);
            app.Screenshot("Navigating to news tab");
            app.Tap(NewsTab);
            return new NewsPage();
        }

        public SourcesPage ShowSourcesTab()
        {
            app.WaitForElement(SourcesTab);
            app.Screenshot("Navigating to sources tab");
            app.Tap(SourcesTab);
            return new SourcesPage();
        }

        public FavoritesPage ShowFavoritesTab()
        {
            app.WaitForElement(FavoritesTab);
            app.Screenshot("Navigating to favorites tab");
            app.Tap(FavoritesTab);
            return new FavoritesPage();
        }

        public SearchPage ShowSearchTab()
        {
            app.WaitForElement(SearchTab);
            app.Screenshot("Navigating to search tab");
            app.Tap(SearchTab);
            return new SearchPage();
        }

        public SettingsPage ShowSettingsTab()
        {
            app.WaitForElement(SettingsTab);
            app.Screenshot("Navigating to settings tab");
            app.Tap(SettingsTab);
            return new SettingsPage();
        }
    }
}
