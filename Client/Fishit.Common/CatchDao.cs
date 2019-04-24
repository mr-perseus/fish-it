using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class CatchDao
    {
        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/catches";

        public async Task<Response<List<Catch>>> GetAllCatches()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK) 
                        return new Response<List<Catch>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetAllCatches",
                            Content = new List<Catch>()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string catchContent = await content.ReadAsStringAsync();
                        return new Response<List<Catch>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetAllCatches",
                            Content = ParseListCatches(catchContent)
                    };
                    }
                }
            }
        }

        public async Task<Response<Catch>> GetCatch()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetAllCatches",
                            Content = new Catch()
                        };
                    using (HttpContent content = response.Content)
                    {
                        string catchContent = await content.ReadAsStringAsync();
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetAllCatches",
                            Content = ParseCatch(catchContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<Catch>> CreateCatch(Catch aCatch)
        {
            StringContent body = new StringContent(StringifyCatch(aCatch), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(EndPointUri + "/new", body))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new Catch()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string catchContent = await responseContent.ReadAsStringAsync();
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseCatch(catchContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<Catch>> UpdateCatch(Catch aCatch)
        {
            StringContent body = new StringContent(StringifyCatch(aCatch), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PutAsync(EndPointUri + "/" + aCatch.CatchId, body))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new Catch()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string catchContent = await responseContent.ReadAsStringAsync();
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseCatch(catchContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<Catch>> DeleteCatch(Catch aCatch)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + aCatch.CatchId))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful GetCatch",
                            Content = new Catch()
                        };
                    using (HttpContent responseContent = response.Content)
                    {
                        string catchContent = await responseContent.ReadAsStringAsync();
                        return new Response<Catch>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful GetCatch",
                            Content = ParseCatch(catchContent)
                        };
                    }
                }
            }
        }

        public List<Catch> ParseListCatches(string catches)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            return JsonConvert.DeserializeObject<List<Catch>>(catches, settings);
        }

        public Catch ParseCatch(string aCatch)
        {
            return JsonConvert.DeserializeObject<Catch>(aCatch);
        }

        public string StringifyCatch(Catch aCatch)
        {
            return JsonConvert.SerializeObject(aCatch);
        }
    }
}