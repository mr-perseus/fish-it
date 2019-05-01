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
            Name = "Browntrout",
            Description = "Salmonid with red and black points on it"
        };

        private readonly Catch _catch = new Catch
        {
            Id = "0",
            DateTime = new DateTime(2019, 05, 01),
            FishType = new FishType(),
            Length = 35,
            Weight = 200
        };

        private readonly FishingTrip _fishingTrip = new FishingTrip
        {
            Id = "0",
            Location = "Lai da Palpougna",
            DateTime = new DateTime(2019, 05, 01),
            Catches = new List<Catch>(),
            Description = "Groundfishing and Livigno-System",
            PredominantWeather = FishingTrip.Weather.Sunny,
            Temperature = 13
        };

        //CRUD Fishtype Section END

        //CRUD Catch Section START
        [Fact]
        public async void CreateCatch()
        {
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);

            _catch.FishType = fishTypes.Content[11];
            Response<Catch> response = await _catchDao.CreateItem(_catch);
            Assert.True(response.StatusCode == HttpStatusCode.OK);

            Response<Catch> responseDeleteCatch = await _catchDao.DeleteItem(response.Content);
            Assert.True(responseDeleteCatch.StatusCode == HttpStatusCode.OK);
        }

        //CRUD Catch Section START

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

        //CRUD Fishtype Section START
        [Fact]
        public async void CreateFishType()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            // in der Response wird im Body nur die generierte _id mitgeschickt, weshalb kein Content geparst werden und verglichen werden kann. Assert.True(_fishType.Name == response.Content.Name);
            await _fishTypeDao.DeleteItem(response
                .Content); //hier zum schluss trotzdem noch Assert.True bez. Response machen? 
        }

        [Fact]
        public async void DeleteFishType()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Response<FishType> responseDelete = await _fishTypeDao.DeleteItem(response.Content);
            Assert.True(responseDelete.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetCatches()
        {
            Response<List<Catch>> allCatchesResponse = await _catchDao.GetAllItems();
            Assert.True(allCatchesResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetFishTypeById()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Response<FishType> selectedFishtype = await _fishTypeDao.GetItemById(response.Content.Id);
            Assert.True(selectedFishtype.Content.Name == "Browntrout");
        }

        [Fact]
        public async void GetFishTypes()
        {
            Response<List<FishType>> response = await _fishTypeDao.GetAllItems();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateFishType()
        {
            _fishType.Name = "Rainbowtrout";
            Response<FishType> response = await _fishTypeDao.UpdateItem(_fishType);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(_fishType.Name == response.Content.Name);
        }
    }
}