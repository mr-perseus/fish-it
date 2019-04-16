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



        //Always OK, not finished test
        [Fact]
        public async void AddFishingTripTest()
        {
            FishingTrip fishingTrip = new FishingTrip
            {
                Id = 0,
                Location = "Zürichsee Glarus",
                DateTime = new DateTime(2019, 04, 16), 
                Description = "Neu POST Versuch",
                PredominantWeather = FishingTrip.Weather.Sunny,
                Temperature = 12.5

               
                /*
                {
                    new Catch {FishType = new Fishtype(), DateTime = new DateTime(2019,04,16,11,25,00),Length = 50, Weight = 100}
                
                } */

            };
            await FishingTripDao.AddFishingTripByWebRequest(fishingTrip);
        }

        [Fact]

        public async void DeleteFishingTripById()
        {
            string existingFishingTripId = "5cb34e41500b0509f4244305";
            await FishingTripDao.DeleteFishingTripByRequest(existingFishingTripId);
        }

        [Fact]
        public void GetListByLocationTest()
        {
            
        }
    }
}