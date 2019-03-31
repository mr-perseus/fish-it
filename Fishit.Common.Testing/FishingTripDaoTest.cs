using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Xunit;

namespace Fishit.Common.Testing
{
    public class FishingTripDaoTest : TestBase
    {
        private FishingTripDao _fishingTripDao;

        private FishingTripDao FishingTripDao =>
            _fishingTripDao ?? (_fishingTripDao = new FishingTripDao());

        [Fact]
        public void AddFishingTripTest()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = 5,
                Name = "Test"
            };

            FishingTripDao.AddFishingTrip(fishingTrip);

            FishingTrip returnedFishingTrip = FishingTripDao.GetById(5);
            Assert.Equal(fishingTrip.Name, returnedFishingTrip.Name);
        }
    }
}