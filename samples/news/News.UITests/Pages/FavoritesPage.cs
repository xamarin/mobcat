using System;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace News.UITests.Pages
{
    public class FavoritesPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(FavoritesPage)),
            iOS = x => x.Marked(nameof(FavoritesPage))
        };

        protected readonly Query FavoriteButton;

        public FavoritesPage()
        {
            FavoriteButton = x => x.Marked(nameof(FavoriteButton));
        }

        public void RemoveFavorite()
        {
            app.WaitForElement(FavoriteButton);
            app.Tap(FavoriteButton);
            app.Screenshot("Removed favorite article");
        }
    }
}
