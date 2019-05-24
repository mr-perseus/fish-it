using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fishit.Common.Properties;
using Fishit.Dal.Entities;
using Fishit.Logging;
using Newtonsoft.Json;

namespace Fishit.Common
{
    public class Dao<T> where T : new()
    {
        private readonly string _endPointUri = Resources.EndPointUri;
        private readonly ILogger _logger;

        public Dao()
        {
            _logger = LogManager.GetLogger(nameof(Dao<T>));
            _endPointUri += typeof(T).Name.ToLower();
        }

        public async Task<Response<List<T>>> GetAllItems()
        {
            _logger.Info(nameof(GetAllItems) + "; Start; ");

            return await HandleRequest<List<T>>(() => new HttpClient().GetAsync(_endPointUri));
        }

        public async Task<Response<T>> GetItem(T item)
        {
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            _logger.Info(nameof(GetItem) + "; Start; " + "id; " + id);

            return await HandleRequest<T>(() =>
                new HttpClient().GetAsync(_endPointUri + Path.DirectorySeparatorChar + id));
        }

        public async Task<Response<T>> GetItemById(string id)
        {
            _logger.Info(nameof(GetItem) + "; Start; " + "id; " + id);

            return await HandleRequest<T>(() =>
                new HttpClient().GetAsync(_endPointUri + Path.DirectorySeparatorChar + id));
        }

        public async Task<Response<T>> CreateItem(T item)
        {
            _logger.Info(nameof(CreateItem) + "; Start; " + "item; " + item);
            StringContent body = new StringContent(Stringify(item), Encoding.UTF8, "application/json");
            // StringContent body = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/octet-stream");

            return await HandleRequest<T>(() =>
                new HttpClient().PostAsync(_endPointUri + Resources.CreateNew, body));
        }

        public async Task<Response<T>> UpdateItem(T item)
        {
            _logger.Info(nameof(UpdateItem) + "; Start; " + "itemBefore; " + item);
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            StringContent body = new StringContent(Stringify(item), Encoding.UTF8, "application/json");

            return await HandleRequest<T>(() =>
                new HttpClient().PutAsync(_endPointUri + Path.DirectorySeparatorChar + id, body));
        }

        public async Task<Response<T>> DeleteItem(T item)
        {
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            _logger.Info(nameof(DeleteItem) + "; Start; " + "id; " + id);

            return await HandleRequest<T>(() =>
                new HttpClient().DeleteAsync(_endPointUri + Path.DirectorySeparatorChar + id));
        }

        public T1 Parse<T1>(string item)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            return JsonConvert.DeserializeObject<T1>(item, settings);
        }

        public string Stringify(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private async Task<Response<T1>> HandleRequest<T1>(Func<Task<HttpResponseMessage>> httpFunction)
            where T1 : new()
        {
            using (HttpResponseMessage response = await httpFunction())
            {
                using (HttpContent content = response.Content)
                {
                    string itemContent = await content.ReadAsStringAsync();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.Error(nameof(HandleRequest) + "; Error; response; " + response);

                        return new Response<T1>
                        {
                            StatusCode = response.StatusCode,
                            Message = itemContent,
                            Content = new T1()
                        };
                    }

                    _logger.Info(nameof(HandleRequest) + "; End; response; " + response + "; itemContent; " +
                                 itemContent);

                    return new Response<T1>
                    {
                        StatusCode = response.StatusCode,
                        Message = "Successful" + typeof(T),
                        Content = Parse<T1>(itemContent)
                    };
                }
            }
        }
    }
}