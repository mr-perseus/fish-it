using System.Collections.Generic;
using System.Linq;
using Fishit.Common;
using Fishit.Dal.Entities;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager
    {
        public List<string> GetAllLocations()
        {
            return GetAllFishingTrips().GroupBy(trip => trip.Location).Select(l => l.Key).ToList();
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