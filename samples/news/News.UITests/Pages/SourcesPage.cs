using System;
namespace News.UITests.Pages
{
    public class SourcesPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked(nameof(SourcesPage)),
            iOS = x => x.Marked(nameof(SourcesPage))
        };

        public SourcesPage()
        {
        }
    }
}
