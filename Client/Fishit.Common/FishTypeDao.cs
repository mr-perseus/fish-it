using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class FishTypeDao
    {
        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishtypes";

        public async Task<Response<List<FishType>>> GetAllFishTypes()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<List<FishType>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful Get All FishTypes",
                            Content = new List<FishType>()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string fishTypeContent = await content.ReadAsStringAsync();
                        return new Response<List<FishType>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetAllCatches",
                            Content = ParseListFishTypes(fishTypeContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<FishType>> GetFishtype()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetAllCatches",
                            Content = new FishType()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string fishTypeContent = await content.ReadAsStringAsync();
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetAllCatches",
                            Content = ParseFishType(fishTypeContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<FishType>> CreateFishType(FishType fishType)
        {
            StringContent body = new StringContent(StringifyFishType(fishType), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(EndPointUri + "/new", body))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new FishType()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string fishTypeContent = await responseContent.ReadAsStringAsync();
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseFishType(fishTypeContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<FishType>> UpdateCatch(FishType fishType)
        {
            StringContent body = new StringContent(StringifyFishType(fishType), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PutAsync(EndPointUri + "/" + fishType.Id, body))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new FishType()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string fishTypeContent = await responseContent.ReadAsStringAsync();
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseFishType(fishTypeContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<FishType>> DeleteCatch(FishType fishType)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + fishType.Id))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new FishType()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string fishTypeContent = await responseContent.ReadAsStringAsync();
                        return new Response<FishType>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseFishType(fishTypeContent)
                        };
                    }
                }
            }
        }

        public List<FishType> ParseListFishTypes(string fishTypes)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            { ContractResolver = new CustomContractResolver() };
            return JsonConvert.DeserializeObject<List<FishType>>(fishTypes, settings);
        }

        public FishType ParseFishType(string fishType)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            { ContractResolver = new CustomContractResolver() };
            return JsonConvert.DeserializeObject<FishType>(fishType, settings);
        }

        public string StringifyFishType(FishType fishType)
        {
            return JsonConvert.SerializeObject(fishType);
        }
    }
}