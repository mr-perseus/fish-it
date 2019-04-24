using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Xunit;

namespace Fishit.Common.Testing
{
    public class FishingTripDaoTest
    {
        private FishingTripDao _fishingTripDao;

        private FishingTripDao FishingTripDao =>
            _fishingTripDao ?? (_fishingTripDao = new FishingTripDao());

        [Fact]
        public async void GetAllFishingTripsAsObjects()
        {
            Response<List<FishingTrip>> httpResponse = await FishingTripDao.GetAllFishingTrips();
            List<FishingTrip> allRegisteredFishingTrips = httpResponse.Content;
            Assert.True(allRegisteredFishingTrips.Count > 0);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            await TestEnvironmentHelper.InitTestData(async id =>
            {
                Response<FishingTrip> httpResponse = await FishingTripDao.GetFishingTripById(id);
                FishingTrip ft = httpResponse.Content;
                Assert.Equal("Neu POST Versuch", ft.Description);
                Assert.Equal("Letzte", ft.Location);
                Assert.Equal(id, ft.Id);
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
            await TestEnvironmentHelper.InitTestData(async id =>
            {
                Response<FishingTrip> httpResponse = await FishingTripDao.GetFishingTripById(id);
                FishingTrip fishingTrip = httpResponse.Content;
                fishingTrip.Temperature = 17;
                await FishingTripDao.UpdateFishingTrip(fishingTrip);

                Assert.Equal(17,
                    (await FishingTripDao.GetFishingTripById(id)).Content.Temperature);
            });
        }
    }
}