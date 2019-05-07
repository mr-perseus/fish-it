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
            Name = "Browntrout",
            Description = "Salmonid with red and black points on it"
        };

        private readonly Catch _catch = new Catch
        {
            DateTime = new DateTime(2019, 05, 01),
            FishType = new FishType(),
            Length = 35,
            Weight = 200
        };

        private readonly FishingTrip _fishingTrip = new FishingTrip
        {
            Location = "Lai da Palpougna",
            DateTime = new DateTime(2019, 05, 01),
            Catches = new List<Catch>(),
            Description = "Groundfishing and Livigno-System",
            PredominantWeather = FishingTrip.Weather.Sunny,
            Temperature = 13
        };

        [Fact]
        public async void CreateCatch()
        {
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);

            _catch.FishType = fishTypes.Content[8];
            Response<Catch> response = await _catchDao.CreateItem(_catch);
            Assert.True(response.StatusCode == HttpStatusCode.OK);

            Response<Catch> responseDeleteCatch = await _catchDao.DeleteItem(response.Content);
            Assert.True(responseDeleteCatch.StatusCode == HttpStatusCode.OK);
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
        public async void CreateFishType()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            await _fishTypeDao.DeleteItem(response.Content);
        }

        [Fact]
        public async void DeleteCatch()
        {
            Response<List<Catch>> countBefore = await _catchDao.GetAllItems();
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);

            _catch.FishType = fishTypes.Content[8];
            Response<Catch> response = await _catchDao.CreateItem(_catch);
            Assert.True(response.StatusCode == HttpStatusCode.OK);

            Response<Catch> responseDelete = await _catchDao.DeleteItem(response.Content);
            Assert.True(responseDelete.StatusCode == HttpStatusCode.OK);
            Response<List<Catch>> countAfter = await _catchDao.GetAllItems();
            Assert.True(countBefore.Content.Count == countAfter.Content.Count);
        }

        [Fact]
        public async void DeleteFishingTrip()
        {
            Response<List<FishingTrip>> countBefore = await _fishingTripDao.GetAllItems();
            Response<FishingTrip> response = await _fishingTripDao.CreateItem(_fishingTrip);
            Response<FishingTrip> responseDelete = await _fishingTripDao.DeleteItem(response.Content);
            Assert.True(responseDelete.StatusCode == HttpStatusCode.OK);
            Response<List<FishingTrip>> countAfter = await _fishingTripDao.GetAllItems();
            Assert.True(countBefore.Content.Count == countAfter.Content.Count);
        }

        [Fact]
        public async void DeleteFishType()
        {
            Response<List<FishType>> countBefore = await _fishTypeDao.GetAllItems();
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Response<FishType> responseDelete = await _fishTypeDao.DeleteItem(response.Content);
            Assert.True(responseDelete.StatusCode == HttpStatusCode.OK);
            Response<List<FishType>> countAfter = await _fishTypeDao.GetAllItems();
            Assert.True(countBefore.Content.Count == countAfter.Content.Count);
        }

        [Fact]
        public async void GetAllCatches()
        {
            Response<List<Catch>> allCatchesResponse = await _catchDao.GetAllItems();
            Assert.True(allCatchesResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetAllFishingTrips()
        {
            Response<List<FishingTrip>> allFishingTripsResponse = await _fishingTripDao.GetAllItems();
            Assert.True(allFishingTripsResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetAllFishTypes()
        {
            Response<List<FishType>> allFishtypesResponse = await _fishTypeDao.GetAllItems();
            Assert.True(allFishtypesResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetCatchById()
        {
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);
            _catch.FishType = fishTypes.Content[8];

            Response<Catch> createdCatchIdResponse = await _catchDao.CreateItem(_catch);
            Response<Catch> selectedCatch = await _catchDao.GetItemById(createdCatchIdResponse.Content.Id);
            Assert.True(selectedCatch.Content.Length == 35);
        }

        [Fact]
        public async void GetFishingTripById()
        {
            Response<FishingTrip> response = await _fishingTripDao.CreateItem(_fishingTrip);
            Response<FishingTrip> selectedFishtype = await _fishingTripDao.GetItemById(response.Content.Id);
            Assert.True(selectedFishtype.Content.Description == "Groundfishing and Livigno-System");
        }

        [Fact]
        public async void GetFishTypeById()
        {
            Response<FishType> response = await _fishTypeDao.CreateItem(_fishType);
            Response<FishType> selectedFishtype = await _fishTypeDao.GetItemById(response.Content.Id);
            Assert.True(selectedFishtype.Content.Name == "Browntrout");
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            Response<List<Catch>> catches = await _catchDao.GetAllItems();
            Assert.True(catches.StatusCode == HttpStatusCode.OK);
            Assert.True(catches.Content.Count > 0);

            _fishingTrip.Catches.Add(catches.Content[0]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripDao.CreateItem(_fishingTrip);
            Assert.True(createdFishingTripResponse.StatusCode == HttpStatusCode.OK);
            Response<FishingTrip> fishingTripBeforeUpdate =
                await _fishingTripDao.GetItemById(createdFishingTripResponse.Content.Id);
            Assert.True((fishingTripBeforeUpdate.Content.Description != "Schwimmer 5m Tiefe"));
            fishingTripBeforeUpdate.Content.Description = "Schwimmer 5m Tiefe";
            fishingTripBeforeUpdate.Content.Location = "Silsersee";
            Response<FishingTrip> updatedFishingTrip =
                await _fishingTripDao.UpdateItem(fishingTripBeforeUpdate.Content);
            Response<FishingTrip> fishingTripAfterUpdate = await _fishingTripDao.GetItemById(updatedFishingTrip.Content.Id);
            Assert.True(fishingTripAfterUpdate.Content.Description == "Schwimmer 5m Tiefe");
            Assert.True(fishingTripAfterUpdate.Content.Location == "Silsersee");
            Response<FishingTrip> deletedFishingtrip = await _fishingTripDao.DeleteItem(fishingTripAfterUpdate.Content);
            Assert.True(deletedFishingtrip.StatusCode == HttpStatusCode.OK);
        }



        [Fact]
        public async void UpdateFishType()
        {
            Response<FishType> createdFishtypeResponse = await _fishTypeDao.CreateItem(_fishType);
            Response<FishType> createdFishtypeByIdResponse = await _fishTypeDao.GetItemById(createdFishtypeResponse.Content.Id);
            Assert.True(createdFishtypeByIdResponse.Content.Name != "Rainbowtrout");
            createdFishtypeByIdResponse.Content.Name = "Rainbowtrout";
            createdFishtypeByIdResponse.Content.Description = "Salmon with rainbow colors";
            Response<FishType> updatedFishtypeResponse = await _fishTypeDao.UpdateItem(createdFishtypeByIdResponse.Content);
            Assert.True(updatedFishtypeResponse.StatusCode == HttpStatusCode.OK);
            Response<FishType> refreshedFishtypeResponse = await _fishTypeDao.GetItemById(updatedFishtypeResponse.Content.Id);
            Assert.True(refreshedFishtypeResponse.Content.Name == "Rainbowtrout");
            Response<FishType> deletedFishtypeResponse = await _fishTypeDao.DeleteItem(refreshedFishtypeResponse.Content);
            Assert.True(deletedFishtypeResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        async void UpdateCatch()
        {
            Response<List<FishType>> fishTypes = await _fishTypeDao.GetAllItems();
            Assert.True(fishTypes.StatusCode == HttpStatusCode.OK);
            Assert.True(fishTypes.Content.Count > 0);
            _catch.FishType = fishTypes.Content[8];

            Response<Catch> createdCatchIdResponse = await _catchDao.CreateItem(_catch);
            Assert.True(createdCatchIdResponse.StatusCode == HttpStatusCode.OK);
            Response<Catch> requestedCatchBeforeUpdateResponse = await _catchDao.GetItemById(createdCatchIdResponse.Content.Id);
            Assert.True(requestedCatchBeforeUpdateResponse.Content.Length != 200);
            requestedCatchBeforeUpdateResponse.Content.Length = 200;

            Response<Catch> refreshedCatchResponse = await _catchDao.UpdateItem(requestedCatchBeforeUpdateResponse.Content);
            Assert.True(refreshedCatchResponse.StatusCode == HttpStatusCode.OK);

            Response<Catch> requestedCatchAfterUpdateResponse = await _catchDao.GetItemById(refreshedCatchResponse.Content.Id);
            Assert.True(requestedCatchAfterUpdateResponse.Content.Length == 200);

            Response<Catch> responseDeleteCatch = await _catchDao.DeleteItem(requestedCatchAfterUpdateResponse.Content);
            Assert.True(responseDeleteCatch.StatusCode == HttpStatusCode.OK);
        }
    }
}