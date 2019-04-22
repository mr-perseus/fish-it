using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Fishit.TestEnvironment.Attributes;
using Xunit;

namespace Fishit.Common.Testing
{
    public class FishingTripDaoTest
    {
        private FishingTripDao _fishingTripDao;

        private FishingTripDao FishingTripDao =>
            _fishingTripDao ?? (_fishingTripDao = new FishingTripDao());

        [SetupTestData]
        [Fact]
        public async void GetAllFishingTripsAsObjects()
        {
            List<FishingTrip> allRegisteredFishingTrips = await FishingTripDao.GetAllFishingTrips();
            Assert.True(allRegisteredFishingTrips.Count > 0);
        }

        [SetupTestData]
        [Fact]
        public async void GetFishingTripById()
        {
            FishingTrip ft = await FishingTripDao.GetFishingTripById(TestEnvironmentHelper.FishingTripId);
            Assert.Equal("Neu POST Versuch", ft.Description);
            Assert.Equal("Letzte", ft.Location);
        }

        [Fact]
        public void GetListByLocationTest()
        {
            // TODO
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            FishingTrip fishingTrip = await FishingTripDao.GetFishingTripById(TestEnvironmentHelper.FishingTripId);
            fishingTrip.Temperature = 17;
            await FishingTripDao.UpdateFishingTrip(fishingTrip);

            Assert.Equal(17, (await FishingTripDao.GetFishingTripById(TestEnvironmentHelper.FishingTripId)).Temperature);
        }
    }
}