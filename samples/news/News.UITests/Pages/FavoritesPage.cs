using System;
using System.Linq;
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
        protected readonly Query EmptyLabel;
        protected readonly Query NewsInfiniteListView;

        public FavoritesPage()
        {
            FavoriteButton = x => x.Marked(nameof(FavoriteButton));
            NewsInfiniteListView = x => x.Marked(nameof(NewsInfiniteListView));
            EmptyLabel = x => x.Marked(nameof(EmptyLabel));
        }

        public FavoritesPage WaitToBecomeEmpty()
        {
            app.Screenshot("Wait to become empty");
            app.WaitForElement(EmptyLabel);
            app.Screenshot("Favorites are empty");
            return this;
        }

        public FavoritesPage WaitToBecomeNotEmpty()
        {
            app.Screenshot("Wait to become not empty");
            app.WaitForNoElement(EmptyLabel);
            app.Screenshot("Favorites are not empty");
            return this;
        }

        public FavoritesPage RefreshFavorites()
        {
            app.WaitForElement(NewsInfiniteListView);
            var newsList = app.Query(NewsInfiniteListView).First();
            app.DragCoordinates(newsList.Rect.X, newsList.Rect.Y, newsList.Rect.X, newsList.Rect.CenterY);
            return this;
        }

        public FavoritesPage RemoveFavorite()
        {
            app.WaitForElement(FavoriteButton);
            app.Tap(FavoriteButton);
            return this;
        }
    }
}
