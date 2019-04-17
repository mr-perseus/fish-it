using System;
using System.Collections.Generic;
using Fishit.Dal.Entities;
using Xunit;

namespace Fishit.Common.Testing
{
    public class FishingTripDaoTest
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
                new Catch
                {
                    FishType = new FishType
                    {
                        FishTypeId = "05cb34f29500b0509f4244306",
                        Name = "Tuna",
                        Description = "Meeresfisch, mit Sonde gefangen"
                    },
                    DateTime = new DateTime(2019, 04, 16, 11, 25, 00),
                    Length = 50,
                    Weight = 100,
                    CatchId = "5cb34f68500b0509f4244307"
                }
            }
        };

        [Fact]
        public async void AddFishingTripTest()
        {
           await  FishingTripDao.CreateFishingTrip(fishingTrip);
        }

        [Fact]
        public async void DeleteFishingTripById()
        {
            string existingFishingTripId = "5cb6db3ebbecba05f472ef20";
            await FishingTripDao.DeleteFishingTrip(existingFishingTripId);
        }

        [Fact]
        public async void GetAllFishingTripsAsObjects()
        {
            List<FishingTrip> allRegisteredFishingTrips = await FishingTripDao.GetAllFishingTrips();
            Assert.True(allRegisteredFishingTrips.Count > 0);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            FishingTrip ft = await FishingTripDao.GetFishingTripById("5cb5929331c4f61570c394be");
            Assert.True(ft.Description == "it was so good");
            Assert.True(ft.Location == "Lago di Maggiore");
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            FishingTrip fishingTripLondon = await FishingTripDao.GetFishingTripById("5cb6d614bbecba05f472ef15");
            fishingTripLondon.Location = "Baggersee Haldenstein";
           await FishingTripDao.UpdateFishingTrip(fishingTripLondon);
        }

        [Fact]
        public void GetListByLocationTest()
        {
        }
    }
}