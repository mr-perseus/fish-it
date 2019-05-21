using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Fishit.Presentation.UI.Testing
{
    [TestFixture(Platform.Android)]
    public class Tests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            _app = AppInitializer.StartApp(_platform);
            _app.Tap(c => c.Marked("Come and get me!"));
            _app.Tap(c => c.Marked("Login"));
            _app.Tap(c => c.Marked("Fishing Trips"));
        }

        private IApp _app;
        private readonly Platform _platform;

        public Tests(Platform platform)
        {
            _platform = platform;
        }


        [Test]
        public void Test01CreateFishingTrip()
        {
            _app.Tap(c => c.Marked("Add Fishing Trip"));

            _app.EnterText(c => c.Marked("Location"), "UITest");
            _app.EnterText(c => c.Marked("Description"), "Test Description");
            _app.Tap(c => c.Marked("Weather"));
            _app.Tap(c => c.Marked("Sunny"));


            AppResult saveButton = _app.Query(c => c.Marked("Save")).FirstOrDefault();
            Assert.NotNull(saveButton);
            if (saveButton != null)
            {
                Assert.True(saveButton.Enabled);
            }

            _app.Tap(c => c.Marked("Save"));

            AppResult[] results = _app.WaitForElement(c => c.Marked("UITest"));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Test02CreateCatch()
        {
            _app.ScrollDown(strategy: ScrollStrategy.Programmatically);
            _app.Tap(c => c.Marked("UITest"));
            _app.Tap(c => c.Marked("View Catches"));
            _app.Tap(c => c.Marked("Add Catch"));
            _app.EnterText(c => c.Marked("Fish Type"), "Spike");
            _app.Tap(c => c.Marked("Spike"));
            _app.EnterText(c => c.Marked("Length"), "50");
            _app.EnterText(c => c.Marked("Weight"), "6");
            _app.Tap(c => c.Marked("Save"));

            AppResult[] results = _app.WaitForElement(c => c.Marked("Spike"));
            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Test03DeleteFishingTrip()
        {
            _app.ScrollDown(strategy: ScrollStrategy.Programmatically);
            _app.Tap(c => c.Marked("UITest"));
            _app.Tap(c => c.Marked("Delete"));
        }
    }
}