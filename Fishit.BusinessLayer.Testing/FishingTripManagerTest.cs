using System.Collections.Generic;
using Fishit.Common;
using Fishit.TestEnvironment;
using Xunit;

namespace Fishit.BusinessLayer.Testing
{
    public class FishingTripManagerTest : TestBase
    {
        private FishingTripManager _fishingTripManager;

        private FishingTripManager FishingTripManager =>
            _fishingTripManager ?? (_fishingTripManager = new FishingTripManager());

        [Fact]
        public void GetAllLocationsTest()
        {
            IList<string> actualList = FishingTripManager.GetAllLocations().ToArray();
        
            string[] expectedList = new string[3] {"Zurich", "Wil", "Geneva"};
        
            Assert.Equal(expectedList, actualList);
        }
    }
}