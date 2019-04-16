using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Fishit.Logging;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class FishingTripDao : DaoBase
    {
        private readonly ILogger _logger;

        public FishingTripDao()
        {
            _logger = LogManager.GetLogger(nameof(DaoBase));
        }

        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishingtrips";

        public async Task<List<FishingTrip>> GetAllFishingTrips()
        {
           // string endPointUri = ConfigurationManager.ConnectionStrings["Setting1"].ConnectionString ?? "Not found";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        List<FishingTrip> allFishingTrips = GetAllFishingTripObjectsFromJson(myContent);
                        string fishingTripId = allFishingTrips.FirstOrDefault()?.Id;
                        return allFishingTrips;
                    }
                }
            }
        }
        
        public FishingTrip GetFishingTripById(string id)
        {
            _logger.Info(nameof(GetFishingTripById) + "; Start; " + "id; " + id);

            FishingTrip fishingTrip = GetAllFishingTrips().Result.FirstOrDefault(entry => entry.Id == id);

            _logger.Info(nameof(GetFishingTripById) + "; End; " + "fishingTrip; " + fishingTrip);

            return fishingTrip;
        }

        public Task<bool> CreateFishingTrip(FishingTrip fishingTrip)
        {
            return Task.Run(() =>
            {
               // var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(EndPointUri + "/new");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string content = ConvertFishingTripObjectToJson(fishingTrip);

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(content);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string result = streamReader.ReadToEnd();
                }

                return true;
            });
        }

        public async Task DeleteFishingTrip(string id)
        {
           // var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + id))
                {
                    using (HttpContent content = response.Content)
                    {
                        string deletedFishingTrip = await content.ReadAsStringAsync();
                    }

                }
            }
        }

        // De-/Serialization of JSON/FishingTrip Objects 

        public List<FishingTrip> GetAllFishingTripObjectsFromJson(string jsonContent)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CustomContractResolver()};
            return JsonConvert.DeserializeObject<List<FishingTrip>>(jsonContent, settings);
        }

        public FishingTrip ConvertJsonToFishingTripObject(string jsonFishingTrip)
        {
            return JsonConvert.DeserializeObject<FishingTrip>(jsonFishingTrip);
        }

        public string ConvertFishingTripObjectToJson(FishingTrip fishingTripObject)
        {
            return JsonConvert.SerializeObject(fishingTripObject);
        }
    }
}