using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Fishit.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class FishingTripDao : DaoBase
    {
        async Task CreateHTTPGetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        GetAllFishingTripObjectsFromJSON(mycontent);
                    }

                }
            }

        }

        public List<FishingTrip> GetAllFishingTripObjectsFromJSON(string jsonContent)
        {

             List<FishingTrip> fishingTrip = JsonConvert.DeserializeObject<List<FishingTrip>>(jsonContent);
             return fishingTrip;
        }

        async Task DeleteFishingTripByRequest(string url, string id)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(url + id))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                    }

                }
            }

        }


        private readonly ILogger _logger;

        public FishingTripDao()
        {
            _logger = LogManager.GetLogger(nameof(DaoBase));
        }

        public void AddFishingTrip(FishingTrip fishingTrip)
        {
            using (FishitContext context = new FishitContext())
            {
                try
                {
                    _logger.Info(nameof(AddFishingTrip) + "; Start; " + "FishingTrip; " + fishingTrip);

                    context.Entry(fishingTrip).State = EntityState.Added;
                    context.SaveChanges();

                    _logger.Info(nameof(AddFishingTrip) + "; End; ");
                }
                catch (DbUpdateException exception)
                {
                    HandleDbUpdateException(exception, context, fishingTrip);
                }
            }
        }

        public FishingTrip GetById(int id)
        {
            _logger.Info(nameof(GetById) + "; Start; " + "id; " + id);

            FishingTrip fishingTrip = GetList().FirstOrDefault(entry => entry.Id == id);

            _logger.Info(nameof(GetById) + "; End; " + "fishingTrip; " + fishingTrip);

            return fishingTrip;
        }

        public IEnumerable<FishingTrip> GetListByLocation(string location)
        {
            _logger.Info(nameof(GetListByLocation) + "; Start; " + "location; " + location);

            IEnumerable<FishingTrip> fishingTrips = GetList().Where(trip => trip.Location == location).ToList();

            _logger.Info(nameof(GetListByLocation) + "; End; " + "fishingTrips.Count; " + fishingTrips.Count());

            return fishingTrips;
        }

        public IEnumerable<FishingTrip> GetList()
        {
            using (FishitContext context = new FishitContext())
            {
                _logger.Debug(nameof(GetList) + "; Start; ");

                IEnumerable<FishingTrip> fishingTrips = context.FishingTrips.ToList();

                _logger.Debug(nameof(GetList) + "; End; " + "fishingTrips.Count; " + fishingTrips.Count());

                return fishingTrips;
            }
        }
    }
}