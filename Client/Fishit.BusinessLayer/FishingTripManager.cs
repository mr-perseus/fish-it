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
            return new List<FishingTrip>();
        }

        public FishingTrip GetFishingTripById(string fishingTripId)
        {
            return new FishingTrip();
        }

        public async void CreateFishingTrip(FishingTrip fishingTrip)
        {
            await new FishingTripDao().AddFishingTripByWebRequest(fishingTrip);
        }

        public async void UpdateFishingTrip(FishingTrip fishingTrip)
        {
            //await new FishingTripDao().UpdateFishingTripByPutRequest(fishingTrip);
        }

        public void DeleteFishingTrip(string fishingTripId)
        {

        }

        public IList<string> GetAllLocations()
        {
            IList<string> locations = GetAllFishingTrips().GroupBy(trip => trip.Location).Select(l => l.Key).ToList();
            
            _logger.Info(nameof(GetAllLocations) + "; locations; "+ locations);

            return locations;
        }

        public IEnumerable<FishingTrip> GetFishingTripsByLocation(string location)
        {
            return new FishingTripDao().GetListByLocation(location);
        }

        public void AddFishingTrip(FishingTrip fishingTrip)
        {
            new FishingTripDao().AddFishingTrip(fishingTrip);
        }
    }
}