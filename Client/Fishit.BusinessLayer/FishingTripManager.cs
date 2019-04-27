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
        private readonly Dao<Catch> _catchDao;
        private readonly Dao<FishingTrip> _fishingTripDao;
        private readonly Dao<FishType> _fishTypeDao;
        private readonly ILogger _logger;

        public FishingTripManager()
        {
            _logger = LogManager.GetLogger(nameof(FishingTripManager));
            _fishTypeDao = new Dao<FishType>("fishTypes");
            _catchDao = new Dao<Catch>("catches");
            _fishingTripDao = new Dao<FishingTrip>("fishingTrips");
        }

        public async Task<Response<List<FishingTrip>>> GetAllFishingTrips()
        {
            return await _fishingTripDao.GetAllItems();
        }

        public async Task<Response<FishingTrip>> GetFishingTrip(FishingTrip fishingTrip)
        {
            return await _fishingTripDao.GetItem(fishingTrip);
        }

        public async Task<Response<FishingTrip>> CreateFishingTrip(FishingTrip fishingTrip)
        {
            return await _fishingTripDao.CreateItem(fishingTrip);
        }

        public async Task<Response<FishingTrip>> UpdateFishingTrip(FishingTrip fishingTrip)
        {
            return await _fishingTripDao.UpdateItem(fishingTrip);
        }

        public async Task<Response<FishingTrip>> DeleteFishingTrip(FishingTrip fishingTrip)
        {
            return await _fishingTripDao.DeleteItem(fishingTrip);
        }

        public async Task<Response<List<Catch>>> GetAllCatches(FishingTrip fishingTrip)
        {
            return await _catchDao.GetAllItems();
        }

        public async Task<Response<FishingTrip>> AddCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Response<Catch> catchResponse = await _catchDao.CreateItem(aCatch);
            if (catchResponse.StatusCode != HttpStatusCode.OK)
                return new Response<FishingTrip>
                {
                    StatusCode = catchResponse.StatusCode,
                    Message = "Unsuccessful addCatch",
                    Content = fishingTrip
                };

            fishingTrip.Catches.Add(catchResponse.Content);
            return new Response<FishingTrip>
            {
                StatusCode = catchResponse.StatusCode,
                Message = "Successful addCatch",
                Content = fishingTrip
            };
        }

        public async Task<Response<FishingTrip>> UpdateCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Response<Catch> catchResponse = await _catchDao.UpdateItem(aCatch);
            if (catchResponse.StatusCode != HttpStatusCode.OK)
                return new Response<FishingTrip>
                {
                    StatusCode = catchResponse.StatusCode,
                    Message = "Unsuccessful update Catch",
                    Content = fishingTrip
                };

            int index = fishingTrip.Catches.IndexOf(aCatch);
            fishingTrip.Catches[index] = catchResponse.Content;
            return new Response<FishingTrip>
            {
                StatusCode = catchResponse.StatusCode,
                Message = "Successful update Catch",
                Content = fishingTrip
            };
        }

        public async Task<Response<FishingTrip>> DeleteCatch(FishingTrip fishingTrip, Catch aCatch)
        {
            Response<Catch> catchResponse = await _catchDao.DeleteItem(aCatch);
            if (catchResponse.StatusCode != HttpStatusCode.OK)
                return new Response<FishingTrip>
                {
                    StatusCode = catchResponse.StatusCode,
                    Message = "Unsuccessful delete Catch",
                    Content = fishingTrip
                };

            int index = fishingTrip.Catches.IndexOf(aCatch);
            fishingTrip.Catches.RemoveAt(index);
            return new Response<FishingTrip>
            {
                StatusCode = catchResponse.StatusCode,
                Message = "Successful delete Catch",
                Content = fishingTrip
            };
        }
    }
}