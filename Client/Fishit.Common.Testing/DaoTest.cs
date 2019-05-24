using System;
using System.Collections.Generic;
using System.Linq;
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
            Name = "Spike",
            Description = "Hunterfish, green marmored"
        };

        private readonly Catch _catch = new Catch
        {
            DateTime = new DateTime(2019, 05, 01),
            FishType = new FishType(),
            Length = 32,
            Weight = 300,
            Image = ""
        };

        private readonly FishingTrip _fishingTrip = new FishingTrip
        {
            Location = "Barrier Lake Barenburg",
            DateTime = new DateTime(2019, 05, 01),
            Catches = new List<Catch>(),
            Description = "Fake flies size 18 to 20, throw next to the inlet",
            PredominantWeather = FishingTrip.Weather.Raining,
            Temperature = 9
        };

        [Fact]
        public async void CreateCatch()
        {
            Response<List<FishType>> allFishTypes = await _fishTypeDao.GetAllItems();
            _catch.FishType = allFishTypes.Content[0];
            Response<List<Catch>> catchListBefore = await _catchDao.GetAllItems();
            Response<Catch> createdCatchResponse = await _catchDao.CreateItem(_catch);
            Response<List<Catch>> catchListAfter = await _catchDao.GetAllItems();
            Assert.True(createdCatchResponse.Content.Weight == _catch.Weight);
            Assert.Equal(createdCatchResponse.Content.Length, catchListAfter.Content.Last().Length);
            Assert.Equal(createdCatchResponse.Content.FishType.ToString(), _catch.FishType.ToString());
            Assert.True(catchListAfter.Content.Count - catchListBefore.Content.Count == 1);
            Response<Catch> deletedCatchResponse = await _catchDao.DeleteItem(createdCatchResponse.Content);
            Assert.True(deletedCatchResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishingTrip()
        {
            Response<List<Catch>> allCatches = await _catchDao.GetAllItems();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            _fishingTrip.Catches.Add(allCatches.Content[2]);
            _fishingTrip.Catches.Add(allCatches.Content[3]);
            Response<List<FishingTrip>> fishingTripListBefore = await _fishingTripDao.GetAllItems();
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripDao.CreateItem(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListAfter = await _fishingTripDao.GetAllItems();
            Assert.True(createdFishingTripResponse.Content.Location == _fishingTrip.Location);
            Assert.True(fishingTripListAfter.Content.Count - fishingTripListBefore.Content.Count == 1);
            Assert.True(createdFishingTripResponse.Content.Catches.Count == 3);
            Response<FishingTrip> deletedFishingTripResponse =
                await _fishingTripDao.DeleteItem(createdFishingTripResponse.Content);
            Assert.True(deletedFishingTripResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishType()
        {
            Response<List<FishType>> fishTypeListBefore = await _fishTypeDao.GetAllItems();
            Response<FishType> createdFishTypeResponse = await _fishTypeDao.CreateItem(_fishType);
            Response<List<FishType>> fishTypeListAfter = await _fishTypeDao.GetAllItems();
            Assert.True(createdFishTypeResponse.Content.Description == _fishType.Description);
            Assert.True(fishTypeListAfter.Content.Count - fishTypeListBefore.Content.Count == 1);
            Response<FishType> deletedFishType = await _fishTypeDao.DeleteItem(createdFishTypeResponse.Content);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteCatch()
        {
            Response<List<FishType>> allFishTypes = await _fishTypeDao.GetAllItems();
            _catch.FishType = allFishTypes.Content[1];
            Response<Catch> createdCatchResponse = await _catchDao.CreateItem(_catch);
            Response<List<Catch>> catchListBefore = await _catchDao.GetAllItems();
            Response<Catch> deletedCatchResponse = await _catchDao.DeleteItem(createdCatchResponse.Content);
            Response<List<Catch>> catchListAfter = await _catchDao.GetAllItems();
            Assert.True(createdCatchResponse.Content.Id == deletedCatchResponse.Content.Id);
            Assert.True(catchListBefore.Content.Count - catchListAfter.Content.Count == 1);
            string lastAddedCatchIdBefore = catchListBefore.Content.Last().Id;
            string lastAddedCatchIdAfter = catchListAfter.Content.Last().Id;
            Assert.True(createdCatchResponse.Content.Id == lastAddedCatchIdBefore);
            Assert.False(createdCatchResponse.Content.Id == lastAddedCatchIdAfter);
        }

        [Fact]
        public async void DeleteFishingTrip()
        {
            Response<List<Catch>> allCatches = await _catchDao.GetAllItems();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripDao.CreateItem(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListBefore = await _fishingTripDao.GetAllItems();
            Response<FishingTrip> deletedFishingTripResponse =
                await _fishingTripDao.DeleteItem(createdFishingTripResponse.Content);
            Response<List<FishingTrip>> fishingTripListAfter = await _fishingTripDao.GetAllItems();
            Assert.True(createdFishingTripResponse.Content.Id == deletedFishingTripResponse.Content.Id);
            Assert.True(fishingTripListBefore.Content.Count - fishingTripListAfter.Content.Count == 1);
            string lastAddedFishingTripIdBefore = fishingTripListBefore.Content.Last().Id;
            string lastAddedFishingTripIdAfter = fishingTripListAfter.Content.Last().Id;
            Assert.True(createdFishingTripResponse.Content.Id == lastAddedFishingTripIdBefore);
            Assert.False(createdFishingTripResponse.Content.Id == lastAddedFishingTripIdAfter);
        }

        [Fact]
        public async void DeleteFishType()
        {
            Response<FishType> createdFishTypeResponse = await _fishTypeDao.CreateItem(_fishType);
            Response<List<FishType>> fishTypeListBefore = await _fishTypeDao.GetAllItems();
            Response<FishType> deletedFishTypeResponse = await _fishTypeDao.DeleteItem(createdFishTypeResponse.Content);
            Response<List<FishType>> fishTypeListAfter = await _fishTypeDao.GetAllItems();
            Assert.True(createdFishTypeResponse.Content.Id == deletedFishTypeResponse.Content.Id);
            Assert.True(fishTypeListBefore.Content.Count - fishTypeListAfter.Content.Count == 1);
            string lastAddedFishingTripIdBefore = fishTypeListBefore.Content.Last().Id;
            string lastAddedFishingTripIdAfter = fishTypeListAfter.Content.Last().Id;
            Assert.True(createdFishTypeResponse.Content.Id == lastAddedFishingTripIdBefore);
            Assert.False(createdFishTypeResponse.Content.Id == lastAddedFishingTripIdAfter);
        }

        [Fact]
        public async void GetAllCatches()
        {
            Response<List<Catch>> catchListBeforeCreate = await _catchDao.GetAllItems();
            Response<List<FishType>> allFishTypes = await _fishTypeDao.GetAllItems();
            _catch.FishType = allFishTypes.Content[0];
            Response<Catch> createdCatchResponse = await _catchDao.CreateItem(_catch);
            Response<List<Catch>> catchListAfterCreate = await _catchDao.GetAllItems();
            Response<Catch> deletedCatchResponse = await _catchDao.DeleteItem(createdCatchResponse.Content);
            Assert.True(catchListAfterCreate.Content.Count - catchListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdCatchResponse.Content.FishType.ToString(),
                catchListAfterCreate.Content.Last().FishType.ToString());
            Assert.Equal(createdCatchResponse.Content.Length, catchListAfterCreate.Content.Last().Length);
            Assert.True(deletedCatchResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetAllFishingTrips()
        {
            Response<List<FishingTrip>> fishingTripListBeforeCreate = await _fishingTripDao.GetAllItems();
            Response<List<Catch>> allCatches = await _catchDao.GetAllItems();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripDao.CreateItem(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListAfterCreate = await _fishingTripDao.GetAllItems();
            Response<FishingTrip> deletedFishingTripResponse =
                await _fishingTripDao.DeleteItem(createdFishingTripResponse.Content);
            Assert.True(fishingTripListAfterCreate.Content.Count - fishingTripListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdFishingTripResponse.Content.Catches.ToString(),
                fishingTripListAfterCreate.Content.Last().Catches.ToString());
            Assert.Equal(createdFishingTripResponse.Content.Location,
                fishingTripListAfterCreate.Content.Last().Location);
            Assert.True(deletedFishingTripResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetAllFishTypes()
        {
            Response<List<FishType>> fishTypeListBeforeCreate = await _fishTypeDao.GetAllItems();
            Response<FishType> createdFishTypeResponse = await _fishTypeDao.CreateItem(_fishType);
            Response<List<FishType>> fishTypeListAfterCreate = await _fishTypeDao.GetAllItems();
            Response<FishType> deletedFishType = await _fishTypeDao.DeleteItem(createdFishTypeResponse.Content);
            Assert.True(fishTypeListAfterCreate.Content.Count - fishTypeListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdFishTypeResponse.Content.Id, fishTypeListAfterCreate.Content.Last().Id);
            Assert.Equal(createdFishTypeResponse.Content.Name, fishTypeListAfterCreate.Content.Last().Name);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        private async void UpdateCatch()
        {
            Response<List<FishType>> allFishTypes = await _fishTypeDao.GetAllItems();
            _catch.FishType = allFishTypes.Content[1];
            Response<Catch> catchBeforeUpdate = await _catchDao.CreateItem(_catch);
            Assert.False(catchBeforeUpdate.Content.Length == 999);
            catchBeforeUpdate.Content.Length = 999;
            Response<Catch> catchAfterUpdate = await _catchDao.UpdateItem(catchBeforeUpdate.Content);
            Assert.Equal(catchBeforeUpdate.Content.Id, catchAfterUpdate.Content.Id);
            Assert.True(catchAfterUpdate.Content.Length == 999);
            Response<Catch> deletedCatchResponse = await _catchDao.DeleteItem(catchAfterUpdate.Content);
            Assert.True(deletedCatchResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            Response<List<Catch>> allCatches = await _catchDao.GetAllItems();
            _fishingTrip.Catches.Add(allCatches.Content[0]);
            Response<FishingTrip> fishingTripBeforeUpdate = await _fishingTripDao.CreateItem(_fishingTrip);
            Assert.False(fishingTripBeforeUpdate.Content.Description == "Schwimmer 5m Tiefe");
            Assert.False(fishingTripBeforeUpdate.Content.Description == "Silsersee");
            Assert.True(fishingTripBeforeUpdate.Content.Catches.Count == 1);
            fishingTripBeforeUpdate.Content.Description = "Schwimmer 5m Tiefe";
            fishingTripBeforeUpdate.Content.Location = "Silsersee";
            fishingTripBeforeUpdate.Content.Catches.Add(allCatches.Content[2]);
            Response<FishingTrip> fishingTripAfterUpdate =
                await _fishingTripDao.UpdateItem(fishingTripBeforeUpdate.Content);
            Assert.Equal(fishingTripBeforeUpdate.Content.Id, fishingTripAfterUpdate.Content.Id);
            Assert.True(fishingTripAfterUpdate.Content.Description == "Schwimmer 5m Tiefe");
            Assert.True(fishingTripAfterUpdate.Content.Location == "Silsersee");
            Assert.True(fishingTripAfterUpdate.Content.Catches.Count == 2);
            Response<FishingTrip> deletedFishingtripResponse =
                await _fishingTripDao.DeleteItem(fishingTripAfterUpdate.Content);
            Assert.True(deletedFishingtripResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateFishType()
        {
            Response<FishType> fishTypeBeforeUpdate = await _fishTypeDao.CreateItem(_fishType);
            Assert.True(fishTypeBeforeUpdate.Content.Name != "Shark");
            Assert.True(fishTypeBeforeUpdate.Content.Description != "Edited Description");
            fishTypeBeforeUpdate.Content.Name = "Shark";
            fishTypeBeforeUpdate.Content.Description = "Edited Description";
            Response<FishType> fishTypeAfterUpdate = await _fishTypeDao.UpdateItem(fishTypeBeforeUpdate.Content);
            Assert.Equal(fishTypeBeforeUpdate.Content.Id, fishTypeAfterUpdate.Content.Id);
            Assert.True(fishTypeAfterUpdate.Content.Name == "Shark");
            Assert.True(fishTypeAfterUpdate.Content.Description == "Edited Description");
            Response<FishType> deletedFishTypeResponse = await _fishTypeDao.DeleteItem(fishTypeAfterUpdate.Content);
            Assert.True(deletedFishTypeResponse.StatusCode == HttpStatusCode.OK);
        }
    }
}