using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Fishit.Dal.Entities;
using Xunit;

namespace Fishit.BusinessLayer.Testing
{
    public class FishingTripManagerTest
    {
        private readonly FishingTripManager _fishingTripManager = new FishingTripManager();

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
            Weight = 300

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
        public async void GetAllFishingTrips()
        {
            Response<List<FishingTrip>> fishingTripListBeforeCreate = await _fishingTripManager.GetAllFishingTrips();
            Response<List<Catch>> allCatches = await _fishingTripManager.GetAllCatches();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListAfterCreate = await _fishingTripManager.GetAllFishingTrips();
            Response<FishingTrip> deletedFishingTripResponse =
                await _fishingTripManager.DeleteFishingTrip(createdFishingTripResponse.Content);
            Assert.True(fishingTripListAfterCreate.Content.Count - fishingTripListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdFishingTripResponse.Content.Catches.ToString(), fishingTripListAfterCreate.Content.Last().Catches.ToString());
            Assert.Equal(createdFishingTripResponse.Content.Location, fishingTripListAfterCreate.Content.Last().Location);
            Assert.True(deletedFishingTripResponse.StatusCode == HttpStatusCode.OK);

        }

        [Fact]
        public async void GetFishingTrip()
        {
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<FishingTrip> fishingTripAfterCreate = await _fishingTripManager.GetFishingTrip(createdFishingTripResponse.Content);
            Response<FishingTrip> deletedFishType = await _fishingTripManager.DeleteFishingTrip(createdFishingTripResponse.Content);
            Assert.Equal(createdFishingTripResponse.Content.Description, fishingTripAfterCreate.Content.Description);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishingTrip()
        {
            Response<List<Catch>> allCatches = await _fishingTripManager.GetAllCatches();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            _fishingTrip.Catches.Add(allCatches.Content[2]);
            _fishingTrip.Catches.Add(allCatches.Content[3]);
            Response<List<FishingTrip>> fishingTripListBefore = await _fishingTripManager.GetAllFishingTrips();
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListAfter = await _fishingTripManager.GetAllFishingTrips();
            Assert.True(createdFishingTripResponse.Content.Location == _fishingTrip.Location);
            Assert.True(fishingTripListAfter.Content.Count - fishingTripListBefore.Content.Count == 1);
            Assert.True(createdFishingTripResponse.Content.Catches.Count == 3);
            Response<FishingTrip> deletedFishingTripResponse =
                await _fishingTripManager.DeleteFishingTrip(createdFishingTripResponse.Content);
            Assert.True(deletedFishingTripResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateFishingTrip()
        {
            Response<List<Catch>> allCatches = await _fishingTripManager.GetAllCatches();
            _fishingTrip.Catches.Add(allCatches.Content[0]);
            Response<FishingTrip> fishingTripBeforeUpdate = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Assert.False(fishingTripBeforeUpdate.Content.Description == "Schwimmer 5m Tiefe");
            Assert.False(fishingTripBeforeUpdate.Content.Description == "Silsersee");
            Assert.True(fishingTripBeforeUpdate.Content.Catches.Count == 1);
            fishingTripBeforeUpdate.Content.Description = "Schwimmer 5m Tiefe";
            fishingTripBeforeUpdate.Content.Location = "Silsersee";
            fishingTripBeforeUpdate.Content.Catches.Add(allCatches.Content[2]);
            Response<FishingTrip> fishingTripAfterUpdate = await _fishingTripManager.UpdateFishingTrip(fishingTripBeforeUpdate.Content);
            Assert.Equal(fishingTripBeforeUpdate.Content.Id, fishingTripAfterUpdate.Content.Id);
            Assert.True(fishingTripAfterUpdate.Content.Description == "Schwimmer 5m Tiefe");
            Assert.True(fishingTripAfterUpdate.Content.Location == "Silsersee");
            Assert.True(fishingTripAfterUpdate.Content.Catches.Count == 2);
            Response<FishingTrip> deletedFishingtripResponse = await _fishingTripManager.DeleteFishingTrip(fishingTripAfterUpdate.Content);
            Assert.True(deletedFishingtripResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteFishingTrip()
        {
            Response<List<Catch>> allCatches = await _fishingTripManager.GetAllCatches();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<List<FishingTrip>> fishingTripListBefore = await _fishingTripManager.GetAllFishingTrips();
            Response<FishingTrip> deletedFishingTripResponse = await _fishingTripManager.DeleteFishingTrip(createdFishingTripResponse.Content);
            Response<List<FishingTrip>> fishingTripListAfter = await _fishingTripManager.GetAllFishingTrips();
            Assert.True(createdFishingTripResponse.Content.Id == deletedFishingTripResponse.Content.Id);
            Assert.True(fishingTripListBefore.Content.Count - fishingTripListAfter.Content.Count == 1);
            string lastAddedFishingTripIdBefore = fishingTripListBefore.Content.Last().Id;
            string lastAddedFishingTripIdAfter = fishingTripListAfter.Content.Last().Id;
            Assert.True(createdFishingTripResponse.Content.Id == lastAddedFishingTripIdBefore);
            Assert.False(createdFishingTripResponse.Content.Id == lastAddedFishingTripIdAfter);
        }

        [Fact]
        public async void GetAllCatches()
        {
            Response<List<Catch>> allCatches = await _fishingTripManager.GetAllCatches();
            _fishingTrip.Catches.Add(allCatches.Content[1]);
            _fishingTrip.Catches.Add(allCatches.Content[2]);
            _fishingTrip.Catches.Add(allCatches.Content[3]);
            Response<FishingTrip> createdFishingTripResponse = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<List<Catch>> catchListBeforeCreate = await _fishingTripManager.GetAllCatches();
            Response<List<FishType>> allFishTypes = await _fishingTripManager.GetAllFishTypes();
            _catch.FishType = allFishTypes.Content[0];
            Response<Catch> createdCatchResponse = await _fishingTripManager.CreateCatch(_catch);
            Response<List<Catch>> catchListAfterCreate = await _fishingTripManager.GetAllCatches();
            Response<FishingTrip> deletedCatchResponse = await _fishingTripManager.DeleteCatch(createdFishingTripResponse.Content, createdFishingTripResponse.Content.Catches.Last());
            Assert.True(createdFishingTripResponse.Content.Catches.Count == 2);
            Assert.True(catchListAfterCreate.Content.Count - catchListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdCatchResponse.Content.FishType.ToString(), catchListAfterCreate.Content.Last().FishType.ToString());
            Assert.Equal(createdCatchResponse.Content.Length, catchListAfterCreate.Content.Last().Length);
            Assert.True(deletedCatchResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void AddCatch()
        {
            
            Response<List<FishType>> allFishTypes = await _fishingTripManager.GetAllFishTypes();
            _catch.FishType = allFishTypes.Content[0];
            Response<Catch> createdCatch = await _fishingTripManager.CreateCatch(_catch);
            Response<FishingTrip> createdFishingTrip = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Assert.True(createdFishingTrip.Content.Catches.Count == 0);
            Response<FishingTrip> addedCatchFishingTrip = await _fishingTripManager.AddCatch(createdFishingTrip.Content, createdCatch.Content );
            Response<FishingTrip> updatedFishingTrip =
                await _fishingTripManager.GetFishingTrip(addedCatchFishingTrip.Content);
           Assert.True(updatedFishingTrip.Content.Catches.Count == 1);
        }

        [Fact]
        public async void UpdateCatch()
        {
            Response<List<FishType>> allFishTypes = await _fishingTripManager.GetAllFishTypes();
            _catch.FishType = allFishTypes.Content[0];
            Response<Catch> catchBeforeUpdate = await _fishingTripManager.CreateCatch(_catch);
            Assert.False(catchBeforeUpdate.Content.Length == 999);
            catchBeforeUpdate.Content.Length = 999;
            Response<FishingTrip> createdFishingTrip = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<FishingTrip> addedCatchFishingTrip = await _fishingTripManager.AddCatch(createdFishingTrip.Content, catchBeforeUpdate.Content);
            Response<FishingTrip> fishingTripAfterUpdate = await _fishingTripManager.UpdateCatch(addedCatchFishingTrip.Content, addedCatchFishingTrip.Content.Catches.Last());
            Assert.Equal(addedCatchFishingTrip.Content.Catches.Last().Id, fishingTripAfterUpdate.Content.Catches.Last().Id);
            Assert.True(fishingTripAfterUpdate.Content.Catches.Last().Length == 999);
            Response<FishingTrip> deletedCatchResponse = await _fishingTripManager.DeleteCatch(fishingTripAfterUpdate.Content, fishingTripAfterUpdate.Content.Catches.Last());
            Assert.True(deletedCatchResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteCatch()
        {
            Response<List<FishType>> allFishTypes = await _fishingTripManager.GetAllFishTypes();
            _catch.FishType = allFishTypes.Content[0];
            Response<Catch> catchBeforeUpdate = await _fishingTripManager.CreateCatch(_catch);
            Assert.False(catchBeforeUpdate.Content.Length == 999);
            catchBeforeUpdate.Content.Length = 999;
            Response<FishingTrip> createdFishingTrip = await _fishingTripManager.CreateFishingTrip(_fishingTrip);
            Response<FishingTrip> addedCatchFishingTrip = await _fishingTripManager.AddCatch(createdFishingTrip.Content, catchBeforeUpdate.Content);
            Response<FishingTrip> fishingTripAfterUpdate = await _fishingTripManager.UpdateCatch(addedCatchFishingTrip.Content, addedCatchFishingTrip.Content.Catches.Last());
            Assert.Equal(addedCatchFishingTrip.Content.Catches.Last().Id, fishingTripAfterUpdate.Content.Catches.Last().Id);
            Assert.True(fishingTripAfterUpdate.Content.Catches.Last().Length == 999);
            Response<FishingTrip> deletedCatchResponse = await _fishingTripManager.DeleteCatch(fishingTripAfterUpdate.Content, fishingTripAfterUpdate.Content.Catches.Last());
            Assert.True(deletedCatchResponse.Content.Catches.Count == 0);
        }

        [Fact]
        public async void GetAllFishTypes()
        {
            Response<List<FishType>> fishTypeListBeforeCreate = await _fishingTripManager.GetAllFishTypes();
            Response<FishType> createdFishTypeResponse = await _fishingTripManager.CreateFishType(_fishType);
            Response<List<FishType>> fishTypeListAfterCreate = await _fishingTripManager.GetAllFishTypes();
            Response<FishType> deletedFishType = await _fishingTripManager.DeleteFishType(createdFishTypeResponse.Content);
            Assert.True(fishTypeListAfterCreate.Content.Count - fishTypeListBeforeCreate.Content.Count == 1);
            Assert.Equal(createdFishTypeResponse.Content.Id, fishTypeListAfterCreate.Content.Last().Id);
            Assert.Equal(createdFishTypeResponse.Content.Name, fishTypeListAfterCreate.Content.Last().Name);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetFishType()
        {
            Response<FishType> createdFishTypeResponse = await _fishingTripManager.CreateFishType(_fishType);
            Response<FishType> fishTypeAfterCreate = await _fishingTripManager.GetFishType(createdFishTypeResponse.Content);
            Response<FishType> deletedFishType = await _fishingTripManager.DeleteFishType(createdFishTypeResponse.Content);
            Assert.Equal(createdFishTypeResponse.Content.Description, fishTypeAfterCreate.Content.Description);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void CreateFishType()
        {
            Response<List<FishType>> fishTypeListBefore = await _fishingTripManager.GetAllFishTypes();
            Response<FishType> createdFishTypeResponse = await _fishingTripManager.CreateFishType(_fishType);
            Response<List<FishType>> fishTypeListAfter = await _fishingTripManager.GetAllFishTypes();
            Assert.True(createdFishTypeResponse.Content.Description == _fishType.Description);
            Assert.True(fishTypeListAfter.Content.Count - fishTypeListBefore.Content.Count == 1);
            Response<FishType> deletedFishType = await _fishingTripManager.DeleteFishType(createdFishTypeResponse.Content);
            Assert.True(deletedFishType.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateFishType()
        {
            Response<FishType> fishTypeBeforeUpdate = await _fishingTripManager.CreateFishType(_fishType);
            Assert.True(fishTypeBeforeUpdate.Content.Name != "Shark");
            Assert.True(fishTypeBeforeUpdate.Content.Description != "Edited Description");
            fishTypeBeforeUpdate.Content.Name = "Shark";
            fishTypeBeforeUpdate.Content.Description = "Edited Description";
            Response<FishType> fishTypeAfterUpdate = await _fishingTripManager.UpdateFishType(fishTypeBeforeUpdate.Content);
            Assert.Equal(fishTypeBeforeUpdate.Content.Id, fishTypeAfterUpdate.Content.Id);
            Assert.True(fishTypeAfterUpdate.Content.Name == "Shark");
            Assert.True(fishTypeAfterUpdate.Content.Description == "Edited Description");
            Response<FishType> deletedFishTypeResponse = await _fishingTripManager.DeleteFishType(fishTypeAfterUpdate.Content);
            Assert.True(deletedFishTypeResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteFishType()
        {
            Response<FishType> createdFishTypeResponse = await _fishingTripManager.CreateFishType(_fishType);
            Response<List<FishType>> fishTypeListBefore = await _fishingTripManager.GetAllFishTypes();
            Response<FishType> deletedFishTypeResponse = await _fishingTripManager.DeleteFishType(createdFishTypeResponse.Content);
            Response<List<FishType>> fishTypeListAfter = await _fishingTripManager.GetAllFishTypes();
            Assert.True(createdFishTypeResponse.Content.Id == deletedFishTypeResponse.Content.Id);
            Assert.True(fishTypeListBefore.Content.Count - fishTypeListAfter.Content.Count == 1);
            string lastAddedFishingTripIdBefore = fishTypeListBefore.Content.Last().Id;
            string lastAddedFishingTripIdAfter = fishTypeListAfter.Content.Last().Id;
            Assert.True(createdFishTypeResponse.Content.Id == lastAddedFishingTripIdBefore);
            Assert.False(createdFishTypeResponse.Content.Id == lastAddedFishingTripIdAfter);
        }

    }
}