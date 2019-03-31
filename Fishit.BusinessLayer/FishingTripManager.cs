using System.Collections.Generic;
using Fishit.Common;
using Fishit.Dal.Entities;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager
    {
        public IEnumerable<FishingTrip> GetListByLocation(string location)
        {
            return new FishingTripDao().GetListByLocation(location);
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