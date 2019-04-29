using System;
using System.Collections.Generic;
using System.Net;
using Fishit.Dal.Entities;
using Xunit;

namespace Fishit.Common.Testing
{
    public class DaoTest
    {
        private readonly Dao<Catch> _catchDao = new Dao<Catch>();
        private readonly Dao<FishType> _fishTypeDao = new Dao<FishType>();
        private readonly Dao<FishingTrip> _fishingTripDao = new Dao<FishingTrip>();

        private readonly FishType _fishType = new FishType
        {
            Id = "0",
            Name = "Shark",
            Description = "A really big and funky fish"
        };

        private readonly Catch _catch = new Catch
        {
            Id = "0",
            DateTime = new DateTime(2019, 04, 17),
            FishType = new FishType(),
            Length = 19.2,
            Weight = 15.8
        };

        private readonly FishingTrip _fishingTrip = new FishingTrip
        {
            Id = "0",
            Location = "Obersee",
            DateTime = new DateTime(2019, 05, 01),
            Catches = new List<Catch>(),
            Description = "What a wonderful Trip.",
            PredominantWeather = FishingTrip.Weather.Overcast,
            Temperature = 18.7
        };

        [Fact]
        public async void CreateCatch()
        {
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);

            _catch.FishType = fishTypes.Content[0];
            Response<Catch> response = await _catchDao.CreateItem(_catch);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishingTrip()
        {
            Response<List<Catch>> catches = await _catchDao.GetAllItems();
            Assert.True(catches.StatusCode == HttpStatusCode.OK);
            Assert.True(catches.Content.Count > 0);

            _fishingTrip.Catches.Add(catches.Content[0]);
            Response<FishingTrip> response = await _fishingTripDao.CreateItem(_fishingTrip);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishTypes()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetFishTypes()
        {
            Response<List<FishType>> response = await _fishTypeDao.GetAllItems();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }
    }
}