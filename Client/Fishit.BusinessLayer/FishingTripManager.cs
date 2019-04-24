using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fishit.Common;
using Fishit.Dal.Entities;
using Fishit.Logging;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager
    {
        private readonly CatchDao _catchDao;
        private readonly ILogger _logger;

        public FishingTripManager()
        {
            _logger = LogManager.GetLogger(nameof(FishingTripManager));
            _catchDao = new CatchDao();
        }

        public async Task<IEnumerable<FishingTrip>> GetAllFishingTrips()
        {
            Response<List<FishingTrip>> httpResonse = await new FishingTripDao().GetAllFishingTrips();
            return httpResonse.Content;
        }

        public async Task<FishingTrip> GetFishingTripById(string fishingTripId)
        {
            Response<FishingTrip> httpResponse = await new FishingTripDao().GetFishingTripById(fishingTripId);
            return httpResponse.Content;
        }

        public async Task<HttpStatusCode> CreateFishingTrip(FishingTrip fishingTrip)
        {
            Response<FishingTrip> httpResponse = await new FishingTripDao().CreateFishingTrip(fishingTrip);
            return httpResponse.StatusCode;
        }

        //public async Task<FishingTrip> UpdateFishingTrip(FishingTrip fishingTrip)
        public async Task<HttpStatusCode> UpdateFishingTrip(FishingTrip fishingTrip)
        {
            Response<FishingTrip> httpResponse = await new FishingTripDao().UpdateFishingTrip(fishingTrip);
            return httpResponse.StatusCode;
            //return new FishingTrip();
        }

        public async Task<bool> DeleteFishingTrip(string fishingTripId)
        {
            await new FishingTripDao().DeleteFishingTrip(fishingTripId);
            return true;
        }

        public async Task<Response<FishingTrip>> AddCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Response<Catch> catchResponse = await _catchDao.CreateCatch(aCatch);
            if (catchResponse.StatusCode != HttpStatusCode.OK)
                return new Response<FishingTrip>
                {
                    StatusCode = catchResponse.StatusCode,
                    Message = "Unsuccessful addCatch",
                    Content = new FishingTrip()
                };

            fishingTrip.Catches.Add(catchResponse.Content);
            return new Response<FishingTrip>
            {
                StatusCode = catchResponse.StatusCode,
                Message = "Successful addCatch",
                Content = fishingTrip
            };
        }

        /*public async Task<FishingTrip> UpdateCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Catch bCatch = await new CatchManager().UpdateCatch(aCatch);
            fishingTrip.Catches.Add(bCatch);
            //return UpdateFishingTrip(fishingTrip);
            return fishingTrip;
        }*/
    }
}