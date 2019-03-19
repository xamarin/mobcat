using System;
using System.Collections.Generic;
using System.Linq;
using News.Helpers;
using NUnit.Framework;
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
        protected readonly Query CategoryLabel;

        public List<string> SupportedCategories => Constants.Categories.All;

        public NewsPage()
        {
            FavoriteButton = x => x.Marked(nameof(FavoriteButton));
            CategoryLabel = x => x.Marked(nameof(CategoryLabel));
        }

        public NewsPage AddFavorite()
        {
            app.WaitForElement(FavoriteButton);
            app.Tap(FavoriteButton);
            app.Screenshot("Added favorite article");
            return this;
        }

        public NewsPage ShowNextCategory()
        {
            app.SwipeRightToLeft(swipeSpeed: 1000);
            app.Screenshot($"Swiped to the next available category");
            return this;
        }

        public NewsPage ValidateCategory(string category)
        {
            app.Screenshot($"Validate category ${category}");
            app.WaitForElement(CategoryLabel);
            var categoryLabel = app.Query(CategoryLabel).First();
            Assert.AreEqual(categoryLabel.Text, category);
            return this;
        }
    }
}