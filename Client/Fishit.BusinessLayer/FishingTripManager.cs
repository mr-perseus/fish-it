using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<FishingTrip>> GetAllFishingTrips()
        {
            return await new FishingTripDao().GetAllFishingTrips();
        }

        public async Task<FishingTrip> GetFishingTripById(string fishingTripId)
        {
            return await new FishingTripDao().GetFishingTripById(fishingTripId);
        }

        public async Task<string> CreateFishingTrip(FishingTrip fishingTrip)
        {
            return await new FishingTripDao().CreateFishingTrip(fishingTrip);
        }

        //public async Task<FishingTrip> UpdateFishingTrip(FishingTrip fishingTrip)
        public async Task<bool> UpdateFishingTrip(FishingTrip fishingTrip)
        {
            return await new FishingTripDao().UpdateFishingTrip(fishingTrip);
            //return new FishingTrip();
        }

        public async Task<bool> DeleteFishingTrip(string fishingTripId)
        {
            await new FishingTripDao().DeleteFishingTrip(fishingTripId);
            return true;
        }

        /*public async Task<FishingTrip> AddCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Catch bCatch = await new CatchManager().CreateCatch(aCatch);
            fishingTrip.Catches.Add(bCatch);
            //return UpdateFishingTrip(fishingTrip);
            return fishingTrip;
        }*/

        /*public async Task<FishingTrip> UpdateCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Catch bCatch = await new CatchManager().UpdateCatch(aCatch);
            fishingTrip.Catches.Add(bCatch);
            //return UpdateFishingTrip(fishingTrip);
            return fishingTrip;
        }*/
    }
}