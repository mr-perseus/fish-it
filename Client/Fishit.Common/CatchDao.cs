using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Fishit.Dal.Entities;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class CatchDao
    {
        private string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/catches";

        public async void CreateCatch(Catch aCatch)
        {
            var body = new StringContent(StringifyCatch(aCatch), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync(EndPointUri, body))
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string createdCatch = await responseContent.ReadAsStringAsync();
                        Console.WriteLine(createdCatch);
                    }
                }
            }
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
