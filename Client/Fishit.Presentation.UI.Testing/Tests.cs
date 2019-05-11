﻿using System.Linq;
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
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = _app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            _app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }
    }
}