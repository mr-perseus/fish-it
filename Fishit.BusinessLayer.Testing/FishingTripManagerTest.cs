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
        public void AddFishingTripTest()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = 1,
                Name = "Test"
            };

            FishingTripManager.Add(fishingTrip);

            FishingTrip returnedFishingTrip = FishingTripManager.GetById(1);
            Assert.Equal(fishingTrip.Name, returnedFishingTrip.Name);
        }
    }
}