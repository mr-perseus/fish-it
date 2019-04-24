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
    public class FishingTripDao
    {
        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishingtrips";
        private readonly ILogger _logger;

        public FishingTripDao()
        {
            _logger = LogManager.GetLogger(nameof(FishingTripDao));
        }

        public async Task<Response<List<FishingTrip>>> GetAllFishingTrips()
        {
            _logger.Info(nameof(GetAllFishingTrips) + "; Start; ");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<List<FishingTrip>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetAllFishingTrips",
                            Content = new List<FishingTrip>()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string catchContent = await content.ReadAsStringAsync();
                        return new Response<List<FishingTrip>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetAllFishingTrips",
                            Content = GetAllFishingTripObjectsFromJson(catchContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<FishingTrip>> GetFishingTripById(string fishingTripId)
        {
            _logger.Info(nameof(GetFishingTripById) + "; Start; " + "id; " + fishingTripId);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response =
                    await client.GetAsync(EndPointUri + Path.DirectorySeparatorChar + fishingTripId))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<FishingTrip>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetFishingTripById",
                            Content = new FishingTrip()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string fishingTripContent = await content.ReadAsStringAsync();
                        return new Response<FishingTrip>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetFishingTripById",
                            Content = ConvertJsonToFishingTripObject(fishingTripContent)
                        };
                    }
                }
            }
        }

        public Task<Response<FishingTrip>> CreateFishingTrip(FishingTrip fishingTrip)
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
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return new Response<FishingTrip>
                    {
                        StatusCode = httpResponse.StatusCode,
                        Message = "Unsuccessful CreateFishingTrip",
                        Content = new FishingTrip()
                    };
                using (StreamReader streamReader =
                    new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string result = streamReader.ReadToEnd();
                    _logger.Info(nameof(CreateFishingTrip) + "; End; " + "createdFishingTripResult; " + result);
                    return new Response<FishingTrip>
                    {
                        StatusCode = httpResponse.StatusCode,
                        Message = "Successful CreateFishingTrip",
                        Content = ConvertJsonToFishingTripObject(result)
                    }; 
                }
            });
        }

        public Task<Response<FishingTrip>> UpdateFishingTrip(FishingTrip fishingTrip)
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
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return new Response<FishingTrip>
                    {
                        StatusCode = httpResponse.StatusCode,
                        Message = "Unsuccessful UpdateFishingTrip",
                        Content = new FishingTrip()
                    };
                using (StreamReader streamReader =
                    new StreamReader(httpResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    string result = streamReader.ReadToEnd();
                    _logger.Info(nameof(UpdateFishingTrip) + "; End; " + "result; " + result);
                    return new Response<FishingTrip>
                    {
                        StatusCode = httpResponse.StatusCode,
                        Message = "Successful UpdateFishingTrip",
                        Content = ConvertJsonToFishingTripObject(result)
                    };
                }


        });
        }

        public async Task<Response<FishingTrip>> DeleteFishingTrip(string id)
        {
            _logger.Info(nameof(DeleteFishingTrip) + "; Start; " + "id; " + id);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + id))
                {
                    if (response.StatusCode != HttpStatusCode.OK) 
                        return new Response<FishingTrip>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful FishingTrip Deletion",
                            Content = new FishingTrip()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string deletedFishingTrip = await content.ReadAsStringAsync();
                        _logger.Info(
                            nameof(UpdateFishingTrip) + "; End; " + "deletedFishingTrip; " + deletedFishingTrip);
                        return new Response<FishingTrip>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful FishingTrip Deletion",
                            Content = ConvertJsonToFishingTripObject(deletedFishingTrip)
                        };
                    }
                }
            }
        }

        public List<FishingTrip> GetAllFishingTripObjectsFromJson(string jsonContent)
        {
            _logger.Info(nameof(GetAllFishingTripObjectsFromJson) + "; Start jsonContent; " + jsonContent);
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            List<FishingTrip> fishingTrips = JsonConvert.DeserializeObject<List<FishingTrip>>(jsonContent, settings);
            _logger.Info(nameof(GetAllFishingTripObjectsFromJson) + "; End fishingTrips.Count; " + fishingTrips.Count);
            return fishingTrips;
        }

        public FishingTrip ConvertJsonToFishingTripObject(string jsonFishingTrip)
        {
            _logger.Info(nameof(ConvertJsonToFishingTripObject) + "; Start; jsonFishingTrip; " + jsonFishingTrip);
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            FishingTrip fishingTrip = JsonConvert.DeserializeObject<FishingTrip>(jsonFishingTrip, settings);
            _logger.Info(nameof(ConvertJsonToFishingTripObject) + "; End fishingTrip; " + fishingTrip);
            return fishingTrip;
        }

        public string ConvertFishingTripObjectToJson(FishingTrip fishingTripObject)
        {
            _logger.Info(nameof(ConvertFishingTripObjectToJson) + "; Start fishingTripObject; " + fishingTripObject);
            string fishingTripString = JsonConvert.SerializeObject(fishingTripObject);
            _logger.Info(nameof(ConvertFishingTripObjectToJson) + "; End fishingTripString; " + fishingTripString);
            return fishingTripString;
        }
    }
}