using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public async Task<List<Catch>> GetAllCatches()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(EndPointUri))
                {
                    using (HttpContent content = response.Content)
                    {
                        string catchContent = await content.ReadAsStringAsync();

                        List<Catch> catches = ParseListCatches(catchContent);
                        string catchId = catches.FirstOrDefault()?.Id;
                        return catches;
                    }
                }
            }
        }

        public async Task<Catch> CreateCatch(Catch aCatch)
        {
            var body = new StringContent(StringifyCatch(aCatch), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(EndPointUri + "/new", body))
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string res = await responseContent.ReadAsStringAsync();
                        return ParseCatch(res);
                    }
                }
            }
        }

        public async Task<Catch> UpdateCatch(Catch aCatch)
        {
            var body = new StringContent(StringifyCatch(aCatch), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PutAsync(EndPointUri + "/" + aCatch.Id, body))
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string res = await responseContent.ReadAsStringAsync();
                        return ParseCatch(res);
                    }
                }
            }
        }

        public async Task<bool> DeleteCatch(Catch aCatch)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.DeleteAsync(EndPointUri + "/" + aCatch.Id))
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string res = await responseContent.ReadAsStringAsync();
                        return true;
                    }
                }
            }
        }

        public List<Catch> ParseListCatches(string catches)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                { ContractResolver = new CustomContractResolver() };
            return JsonConvert.DeserializeObject<List<Catch>>(catches, settings);
        }
        public Catch ParseCatch(string aCatch)
        {
            return (Catch) JsonConvert.DeserializeObject<Catch>(aCatch);
        }
        public string StringifyCatch(Catch aCatch)
        {
            return JsonConvert.SerializeObject(aCatch);
        }
    }
}
