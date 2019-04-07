using System.Collections.Generic;
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

        public IEnumerable<FishingTrip> GetAllFishingTrips()
        {
            return new FishingTripDao().GetList();
        }

        public void AddFishingTrip(FishingTrip fishingTrip)
        {
            new FishingTripDao().AddFishingTrip(fishingTrip);
        }
    }
}