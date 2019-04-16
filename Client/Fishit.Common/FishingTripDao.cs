using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Fishit.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Configuration;


namespace Fishit.Common
{
    public class FishingTripDao : DaoBase
    {
        
       private string endPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishingtrips";

        public async Task<List<FishingTrip>> GetListOfAllFishingTripObjects()
        {
           // string endPointUri = ConfigurationManager.ConnectionStrings["Setting1"].ConnectionString ?? "Not found";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(endPointUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        List<FishingTrip> allFishingTrips = GetAllFishingTripObjectsFromJson(mycontent);
                        string FishingTripId = allFishingTrips.FirstOrDefault().Id;
                        return allFishingTrips;
                        

                    }

                }
            }

        }

        public async Task AddFishingTripByPostRequest(FishingTrip fishingtrip)
        {
          //  var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
            string content = ConvertFishingTripObjectToJson(fishingtrip);
            var contentForRequest = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
            //var contentForRequest = new StringContent(content.ToString()); should be same like code above
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(endPointUri+"/new", contentForRequest))
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string addedFishingTrip = await responseContent.ReadAsStringAsync();
                    }

                }
            }
        }

        // Second Way to Add/Create Fishing Trips
        public async Task AddFishingTripByWebRequest(FishingTrip fishingtrip)
        {
           // var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPointUri+"/new");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string content = ConvertFishingTripObjectToJson(fishingtrip);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }



        public async Task DeleteFishingTripByRequest(string id)
        {
           // var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(endPointUri+"/" + id))
                {
                    using (HttpContent content = response.Content)
                    {
                        string deletedFishingTrip = await content.ReadAsStringAsync();
                    }

                }
            }
        }

        public FishingTrip GetFishingTripById(string id)
        {
            _logger.Info(nameof(GetFishingTripById) + "; Start; " + "id; " + id);

            FishingTrip fishingTrip = GetListOfAllFishingTripObjects().Result.FirstOrDefault(entry => entry.Id == id);

            _logger.Info(nameof(GetFishingTripById) + "; End; " + "fishingTrip; " + fishingTrip);

            return fishingTrip;
        }

        // De-/Serialization of JSON/FishingTrip Objects 

        public List<FishingTrip> GetAllFishingTripObjectsFromJson(string jsonContent)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CustomContractResolver();
            List<FishingTrip> fishingTripList = JsonConvert.DeserializeObject<List<FishingTrip>>(jsonContent, settings);
            return fishingTripList;
        }

        public FishingTrip ConvertJsonToFishingTripObject(string jsonFishingTrip)
        {
            FishingTrip fishingtripObject = JsonConvert.DeserializeObject<FishingTrip>(jsonFishingTrip);
            return fishingtripObject;
        }
        public string ConvertFishingTripObjectToJson(FishingTrip fishingtripObject)
        {
            var jsonFishingTrip = JsonConvert.SerializeObject(fishingtripObject);
            return jsonFishingTrip;
        }

        //OLD CODE v (DOWN) ----  NEW CODE ^(UP)

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