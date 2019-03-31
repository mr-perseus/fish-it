using System.Collections.Generic;
using System.Linq;
using Fishit.Dal.Entities;
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
        public void GetListByLocationTest()
        {
            IEnumerable<FishingTrip> actualList = FishingTripManager.GetListByLocation("Wil").ToList();

            Assert.Single(actualList);

            Assert.Equal(2, actualList.First().Id);
            Assert.Equal("FishingTrip Number 2", actualList.First().Name);
            Assert.Equal("Wil", actualList.First().Location);
        }
    }
}