using NUnit.Framework;
using Weather.UITests.Pages;
using Xamarin.UITest;

namespace Weather.UITests
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void SetLocationRedmond()
        {
            new MainPage().SetLocationToRedmond();
        }

        [Test]
        public void Repl()
        {
            if (TestEnvironment.IsTestCloud)
            {
                Assert.Ignore("Local only");
            }

            app.Repl();
        }
    }
}
