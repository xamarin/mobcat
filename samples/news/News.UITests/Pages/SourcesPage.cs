using System;
using System.Collections.Generic;
using System.Linq;
using News.Helpers;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace News.UITests.Pages
{
    public class SourcesPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(SourcesPage)),
            iOS = x => x.Marked(nameof(SourcesPage))
        };

        protected readonly Query SourceLabel;

        public List<string> SupportedSources => Constants.Sources.All;

        public SourcesPage()
        {
            SourceLabel = x => x.Marked(nameof(SourceLabel));
        }

        public SourcesPage ShowNextSource()
        {
            app.SwipeRightToLeft(swipeSpeed: 1000);
            app.Screenshot($"Swiped to the next available source");
            return this;
        }

        public SourcesPage ValidateSource(string source)
        {
            app.Screenshot($"Validate source ${source}");
            app.WaitForElement(SourceLabel);
            var sourceLabel = app.Query(SourceLabel).First();
            Assert.AreEqual(sourceLabel.Text, source);
            return this;
        }
    }
}
