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
        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishingtrips";
        private readonly ILogger _logger;

        public FishingTripDao()
        {
            _logger = LogManager.GetLogger(nameof(DaoBase));
        }

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

        public async Task<FishingTrip> GetFishingTripById(string fishingTripId)
        {
            _logger.Info(nameof(GetFishingTripById) + "; Start; " + "id; " + fishingTripId);

            // FishingTrip fishingTrip = GetAllFishingTrips().Result.FirstOrDefault(entry => entry.Id == id);
            // TODO async http request using Endpoint + "/" + id
            //_logger.Info(nameof(GetFishingTripById) + "; End; " + "fishingTrip; " + fishingTrip);

            return new FishingTrip();
        }

        public async Task CreateFishingTrip(FishingTrip fishingTrip)
        {
           
                // var endPointUri = ConfigurationManager.ConnectionStrings["GetFishingTripUri"].ConnectionString;
                HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(EndPointUri + "/new");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string content = ConvertFishingTripObjectToJson(fishingTrip);

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(content);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (StreamReader streamReader =
                    new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string result = streamReader.ReadToEnd();
                }

             
        }

        public async Task<FishingTrip> UpdateFishingTrip(FishingTrip fishingTrip)
        {
            string Id = fishingTrip.Id;
            // TODO async http request using Endpoint + "/" + id
            return new FishingTrip();
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
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
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