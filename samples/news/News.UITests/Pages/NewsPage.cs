using System;
using System.Linq;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace News.UITests.Pages
{
    public class NewsPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(NewsPage)),
            iOS = x => x.Marked(nameof(NewsPage))
        };

        protected readonly Query FavoriteButton;

        public NewsPage()
        {
            FavoriteButton = x =>x.Marked(nameof(FavoriteButton));
        }

        public void AddFavorite()
        {
            app.WaitForElement(FavoriteButton);
            app.Tap(FavoriteButton);
            app.Screenshot("Added favorite article");
        }
    }
}
