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

        [Fact]
        public async void AddFishingTripByRequestTestAsync()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Name = "Test",
                Location = "Sufnersee",
                DateTime = new DateTime(2019, 04, 14), 
                Description = "Erster POST Versuch",
                PredominantWeather = FishingTrip.Weather.Hailing,
                Temperature = 12.5,
                Catches = new List<Catch>()

            };
            await FishingTripDao.AddFishingTripByPostRequest(fishingTrip);

            FishingTrip returnedFishingTrip = FishingTripDao.GetById(5);
            Assert.Equal(fishingTrip.Name, returnedFishingTrip.Name);
        }

        [Fact]
        public void GetListByLocationTest()
        {
            IEnumerable<FishingTrip> actualList = FishingTripDao.GetListByLocation("Wil").ToList();

            Assert.Single(actualList);

            Assert.Equal(2, actualList.First().Id);
            Assert.Equal("FishingTrip Number 2", actualList.First().Name);
            Assert.Equal("Wil", actualList.First().Location);
        }
    }
}