using System;
using System.Collections.Generic;
using System.IO;
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
            _logger.Info(nameof(GetAllFishingTrips) + "; Start; ");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        List<FishingTrip> allFishingTrips = GetAllFishingTripObjectsFromJson(myContent);
                        _logger.Info(nameof(GetAllFishingTrips) + "; End; ");
                        return allFishingTrips;
                    }
                }
            }
        }

        public async Task<FishingTrip> GetFishingTripById(string fishingTripId)
        {
            _logger.Info(nameof(GetFishingTripById) + "; Start; " + "id; " + fishingTripId);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response =
                    await client.GetAsync(EndPointUri + Path.DirectorySeparatorChar + fishingTripId))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        FishingTrip fishingTrip = ConvertJsonToFishingTripObject(myContent);
                        _logger.Info(nameof(GetFishingTripById) + "; End; " + "fishingTrip; " + fishingTrip);
                        return fishingTrip;
                    }
                }
            }
        }

        public Task<bool> CreateFishingTrip(FishingTrip fishingTrip)
        {
            return Task.Run(() =>
            {
                _logger.Info(nameof(CreateFishingTrip) + "; Start; " + "fishingTrip; " + fishingTrip);
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
                    _logger.Info(nameof(CreateFishingTrip) + "; End; " + "createdFishingTripResult; " + result);
                }

                return true;
            });
        }

        public Task<bool> UpdateFishingTrip(FishingTrip fishingTrip)
        {
            return Task.Run(() =>
            {
                _logger.Info(nameof(UpdateFishingTrip) + "; Start; " + "fishingTripbefore; " + fishingTrip);
                HttpWebRequest httpWebRequest =
                    (HttpWebRequest) WebRequest.Create(EndPointUri + Path.DirectorySeparatorChar + fishingTrip.Id);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

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
                    _logger.Info(nameof(UpdateFishingTrip) + "; End; " + "result; " + result);
                }

                return true;
            });
        }

        public async Task DeleteFishingTrip(string id)
        {
            _logger.Info(nameof(DeleteFishingTrip) + "; Start; " + "id; " + id);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + id))
                {
                    using (HttpContent content = response.Content)
                    {
                        string deletedFishingTrip = await content.ReadAsStringAsync();
                        _logger.Info(
                            nameof(UpdateFishingTrip) + "; End; " + "deletedFishingTrip; " + deletedFishingTrip);
                    }
                }
            }
        }


        public List<FishingTrip> GetAllFishingTripObjectsFromJson(string jsonContent)
        {
            _logger.Info(nameof(GetAllFishingTripObjectsFromJson) + "; Start; ");
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            _logger.Info(nameof(GetAllFishingTripObjectsFromJson) + "; End; ");
            return JsonConvert.DeserializeObject<List<FishingTrip>>(jsonContent, settings);
        }

        public FishingTrip ConvertJsonToFishingTripObject(string jsonFishingTrip)
        {
            _logger.Info(nameof(ConvertJsonToFishingTripObject) + "; Start; ");
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            _logger.Info(nameof(ConvertJsonToFishingTripObject) + "; End; ");
            return JsonConvert.DeserializeObject<FishingTrip>(jsonFishingTrip, settings);
        }

        public string ConvertFishingTripObjectToJson(FishingTrip fishingTripObject)
        {
            _logger.Info(nameof(ConvertFishingTripObjectToJson) + "; Start and End; ");
            return JsonConvert.SerializeObject(fishingTripObject);
        }
    }
}