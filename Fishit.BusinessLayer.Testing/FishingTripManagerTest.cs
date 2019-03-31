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
            IList<string> actualList = FishingTripManager.GetAllLocations();
        


        }
    }
}