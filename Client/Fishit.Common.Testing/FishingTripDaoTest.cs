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

        FishingTrip fishingTrip = new FishingTrip
        {
            
            Location = "Letzte",
            DateTime = new DateTime(2019, 04, 16),
            Description = "Neu POST Versuch",
            PredominantWeather = FishingTrip.Weather.Sunny,
            Temperature = 12.5,
            Catches = 
                {
                    new Catch {FishType = new FishType
                    {   Id = "0",
                        Name = "Tuna", 
                        Description = "Meeresfisch, mit Sonde gefangen"
                    },
                    DateTime = new DateTime(2019,04,16,11,25,00),
                    Length = 50,
                    Weight = 100,
                    Id = "0"
                    }
                }
        };

        [Fact]
        public async void AddFishingTripTest()
        {
            await FishingTripDao.CreateFishingTrip(fishingTrip);
        }

        [Fact]
        public async void DeleteFishingTripById()
        {
            string existingFishingTripId = "5cb34e41500b0509f4244305";
            await FishingTripDao.DeleteFishingTrip(existingFishingTripId);
        }

        [Fact]
        public async void GetAllFishingTripsAsObjects()
        {
            List<FishingTrip> allRegisteredFishingTrips = await FishingTripDao.GetAllFishingTrips();
            Assert.True(allRegisteredFishingTrips.Count == 42);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            FishingTrip ft = await FishingTripDao.GetFishingTripById("5cb5d242c5d4d81b1863c34e");
            Assert.True(ft.Description == "Good trip");
            Assert.True(ft.Location == "Type");
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
          FishingTrip fishingTripLondon = await FishingTripDao.GetFishingTripById("5cb34e19500b0509f4244304");
          fishingTripLondon.Description = "it was so good";
          await FishingTripDao.UpdateFishingTrip(fishingTripLondon);
        }

        [Fact]
        public void GetListByLocationTest()
        {
        }
    }
}