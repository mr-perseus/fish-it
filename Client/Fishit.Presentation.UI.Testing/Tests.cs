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
        }

        private IApp _app;
        private readonly Platform _platform;

        public Tests(Platform platform)
        {
            _platform = platform;
        }

        [Test]
        public void CreateFishingTrip()
        {
            _app.Tap(c => c.Marked("Come and get me!"));
            _app.Tap(c => c.Marked("Login"));
            _app.Tap(c => c.Marked("Fishing Trips"));
            _app.Tap(c => c.Marked("Add Fishing Trip"));

            _app.EnterText(c => c.Marked("Location"), "Zurich");
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
        }
    }
}