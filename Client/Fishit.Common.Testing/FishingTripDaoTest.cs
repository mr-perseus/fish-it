using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Xunit;

namespace Fishit.Common.Testing
{
    public class FishingTripDaoTest
    {
        private Dao<FishingTrip> _fishingTripDao;

        private Dao<FishingTrip> FishingTripDao =>
            _fishingTripDao ?? (_fishingTripDao = new Dao<FishingTrip>("fishingTrips"));

        [Fact]
        public async void GetAllFishingTripsAsObjects()
        {
            Response<List<FishingTrip>> httpResponse = await FishingTripDao.GetAllItems();
            List<FishingTrip> allRegisteredFishingTrips = httpResponse.Content;
            Assert.True(allRegisteredFishingTrips.Count > 0);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            await TestEnvironmentHelper.InitTestData(async fishingTrip =>
            {
                Response<FishingTrip> httpResponse = await FishingTripDao.GetItem(fishingTrip);
                FishingTrip ft = httpResponse.Content;
                Assert.Equal("Neu POST Versuch", ft.Description);
                Assert.Equal("Letzte", ft.Location);
                Assert.Equal(fishingTrip.Id, ft.Id);
            });
        }

        [Fact]
        public void GetListByLocationTest()
        {
            // TODO
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            await TestEnvironmentHelper.InitTestData(async generatedTrip =>
            {
                Response<FishingTrip> httpResponse = await FishingTripDao.GetItem(generatedTrip);
                FishingTrip fishingTrip = httpResponse.Content;
                fishingTrip.Temperature = 17;
                await FishingTripDao.GetItem(fishingTrip);

                Assert.Equal(17,
                    (await FishingTripDao.GetItem(generatedTrip)).Content.Temperature);
            });
        }
    }
}