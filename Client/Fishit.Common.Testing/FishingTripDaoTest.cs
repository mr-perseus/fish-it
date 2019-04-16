using System;
using System.Collections.Generic;
using System.Linq;
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
        //Trouble with FishType(JSON) convert to Fishtype(C#)
        public async void GetAllFishingTripsAsObjects()
        {
          List<FishingTrip> allRegisteredFishingTrips = await FishingTripDao.GetListOfAllFishingTripObjects();
          Assert.True(allRegisteredFishingTrips.Count == 6);
        }

        [Fact]
        public void AddFishingTripTest()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = 5,
                Location = "Test"
            };

            FishingTripDao.AddFishingTrip(fishingTrip);

            FishingTrip returnedFishingTrip = FishingTripDao.GetById(5);
            Assert.Equal(fishingTrip.Id, returnedFishingTrip.Id);
        }

        //Always OK, not finished test
        [Fact]
        public async void AddFishingTripByRequestTestAsync()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = 0,
                Location = "Lago di Maggiore",
                DateTime = new DateTime(2019, 04, 14), 
                Description = "Catchlist POST Versuch",
                PredominantWeather = FishingTrip.Weather.Hailing,
                Temperature = 12.5,
                Catches = new List<Catch>()

            };
            await FishingTripDao.AddFishingTripByWebRequest(fishingTrip);

            FishingTrip returnedFishingTrip = FishingTripDao.GetById(5);
            Assert.Equal(fishingTrip.Id, returnedFishingTrip.Id);
        }

        [Fact]
        public void GetListByLocationTest()
        {
            IEnumerable<FishingTrip> actualList = FishingTripDao.GetListByLocation("Wil").ToList();

            Assert.Single(actualList);

            Assert.Equal(2, actualList.First().Id);
            Assert.Equal("Wil", actualList.First().Location);
        }
    }
}