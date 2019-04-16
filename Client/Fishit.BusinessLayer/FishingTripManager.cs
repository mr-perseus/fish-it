using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fishit.Common;
using Fishit.Dal.Entities;
using Fishit.Logging;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager
    {
        private readonly ILogger _logger;

        public FishingTripManager()
        {
            _logger = LogManager.GetLogger(nameof(FishingTripManager));
        }

        public IEnumerable<FishingTrip> GetAllFishingTrips()
        {
            List<FishingTrip> allFishingTrips = new FishingTripDao().GetListOfAllFishingTripObjects().Result;
            return allFishingTrips;
        }

        public FishingTrip GetFishingTripById(string fishingTripId)
        {
            FishingTrip selectedFishingTrip = GetFishingTripById(fishingTripId);
            return selectedFishingTrip;
        }

        public async void CreateFishingTrip(FishingTrip fishingTrip)
        {
            await new FishingTripDao().AddFishingTripByWebRequest(fishingTrip);
        }

        public async void UpdateFishingTrip(FishingTrip fishingTrip)
        {
            //await new FishingTripDao().UpdateFishingTripByPutRequest(fishingTrip);
        }

        public async void DeleteFishingTrip(string fishingTripId)
        {
            await new FishingTripDao().DeleteFishingTripByRequest(fishingTripId);
        }

        public IList<string> GetAllLocations()
        {
            IList<string> locations = GetAllFishingTrips().GroupBy(trip => trip.Location).Select(l => l.Key).ToList();
            
            _logger.Info(nameof(GetAllLocations) + "; locations; "+ locations);

            return locations;
        }
    }
}