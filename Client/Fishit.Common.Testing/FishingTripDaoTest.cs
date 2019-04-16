using System;
using System.Collections.Generic;
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

        //Always OK, not finished test
        [Fact]
        public async void AddFishingTripTest()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = "Zero",
                Location = "TestHuzaaa",
                DateTime = new DateTime(2019, 04, 16),
                Description = "Neu POST Versuch",
                PredominantWeather = FishingTrip.Weather.Sunny,
                Temperature = 12.5


                /*
                {
                    new Catch {FishType = new Fishtype(), DateTime = new DateTime(2019,04,16,11,25,00),Length = 50, Weight = 100}
                
                } */
            };
            await FishingTripDao.CreateFishingTrip(fishingTrip);
        }

        [Fact]
        public async void DeleteFishingTripById()
        {
            string existingFishingTripId = "5cb34e41500b0509f4244305";
            await FishingTripDao.DeleteFishingTrip(existingFishingTripId);
        }

        [Fact]
        //Trouble with FishType(JSON) convert to Fishtype(C#)
        public async void GetAllFishingTripsAsObjects()
        {
            List<FishingTrip> allRegisteredFishingTrips = await FishingTripDao.GetAllFishingTrips();
            Assert.True(allRegisteredFishingTrips.Count == 27);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            FishingTrip ft = await FishingTripDao.GetFishingTripById("5cb5d242c5d4d81b1863c34e");
            Assert.True(ft.Description == "Good trip");
            Assert.True(ft.Location == "Type");
        }

        [Fact]
        public void GetListByLocationTest()
        {
        }
    }
}