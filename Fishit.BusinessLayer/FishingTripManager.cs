using System.Collections.Generic;
using System.Linq;
using Fishit.Common;
using Fishit.Dal.Entities;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager
    {
        public IEnumerable<FishingTrip> GetListByLocation(string location)
        {
            return GetList().Where(trip => trip.Location == location);

        }

        public IEnumerable<FishingTrip> GetList()
        {
            return new FishingTripDao().GetList();
        }

        public void AddFishingTrip(FishingTrip fishingTrip)
        {
            new FishingTripDao().AddFishingTrip(fishingTrip);
        }
    }
}
