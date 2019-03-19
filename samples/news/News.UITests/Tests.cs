using System;
using System.IO;
using System.Linq;
using News.UITests.Pages;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace News.UITests
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void ViewEachTabInDarkModeAndLightMode()
        {
            var homePage = new HomePage();
            homePage.ShowNewsTab();
            homePage.ShowSourcesTab();
            homePage.ShowFavoritesTab();
            homePage.ShowSearchTab();
            homePage.ShowSettingsTab()
            .SetLightMode();

            homePage.ShowNewsTab();
            homePage.ShowSourcesTab();
            homePage.ShowFavoritesTab();
            homePage.ShowSearchTab();
        }

        [Test]
        public void SearchForMicrosoftNews()
        {
            new HomePage()
                .ShowSearchTab()
                .Search("Microsoft");
        }

        [Test]
        [Ignore] //Remove ignore tag after client secrets are implemented on DevOps
        public void AddAndRemoveFavorite()
        {
            var homePage = new HomePage();

            homePage
            .ShowNewsTab()
            .AddFavorite();

            homePage
            .ShowFavoritesTab()
            .RemoveFavorite();
        }

        [Test]
        public void SetLightMode()
        {
            new HomePage()
                .ShowSettingsTab()
                .SetLightMode();
        }

        [Test]
        public void SwipeThroughEachCategory()
        {
            var homePage = new HomePage();
            var newsPage = homePage.ShowNewsTab();

            foreach (var selectedCategory in newsPage.SupportedCategories)
            {
                newsPage.ValidateCategory(selectedCategory);
                newsPage.ShowNextCategory();
            }
        }

        [Test]
        public void SwipeThroughEachSource()
        {
            new HomePage().ShowSourcesTab();

            foreach (var selectedSource in new SourcesPage().SupportedSources)
            {
                new SourcesPage()
                    .ValidateSource(selectedSource)
                    .ShowNextSource();
            }
        }

        [Test]
        [Ignore]
        public void Repl()
        {
            if (TestEnvironment.IsTestCloud)
                Assert.Ignore("Local only");

            app.Repl();
        }
    }
}
