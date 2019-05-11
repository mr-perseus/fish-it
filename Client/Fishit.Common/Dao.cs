﻿using System;
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
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(_endPointUri))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.Error(nameof(GetAllItems) + "; Error; response; " + response);

                        return new Response<List<T>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Unsuccessful Get All " + typeof(T),
                            Content = new List<T>()
                        };
                    }

                    using (HttpContent content = response.Content)
                    {
                        string itemContent = await content.ReadAsStringAsync();
                        _logger.Info(nameof(GetAllItems) + "; End; response; " + response + "; itemContent; " +
                                     itemContent);

                        return new Response<List<T>>
                        {
                            StatusCode = response.StatusCode,
                            Message = "Successful Get All " + typeof(T),
                            Content = ParseList(itemContent)
                        };
                    }
                }
            }
        }

        public async Task<Response<T>> GetItem(T item)
        {
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            _logger.Info(nameof(GetItem) + "; Start; " + "id; " + id);

            using (HttpClient client = new HttpClient())
            {
                return await HandleRequest(() => client.GetAsync(_endPointUri + Path.DirectorySeparatorChar + id));
            }
        }

        public async Task<Response<T>> GetItemById(string id)
        {
            _logger.Info(nameof(GetItem) + "; Start; " + "id; " + id);

            using (HttpClient client = new HttpClient())
            {
                return await HandleRequest(() => client.GetAsync(_endPointUri + Path.DirectorySeparatorChar + id));
            }
        }

        public async Task<Response<T>> CreateItem(T item)
        {
            _logger.Info(nameof(CreateItem) + "; Start; " + "item; " + item);
            StringContent body = new StringContent(Stringify(item), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                return await HandleRequest(() =>
                    client.PostAsync(_endPointUri + Resources.CreateNew, body));
            }
        }

        public async Task<Response<T>> UpdateItem(T item)
        {
            _logger.Info(nameof(UpdateItem) + "; Start; " + "itemBefore; " + item);
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            StringContent body = new StringContent(Stringify(item), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                return await HandleRequest(() =>
                    client.PutAsync(_endPointUri + Path.DirectorySeparatorChar + id, body));
            }
        }

        public async Task<Response<T>> DeleteItem(T item)
        {
            string id = typeof(T).GetProperty("Id")?.GetValue(item).ToString();
            _logger.Info(nameof(DeleteItem) + "; Start; " + "id; " + id);

            using (HttpClient client = new HttpClient())
            {
                return await HandleRequest(() => client.DeleteAsync(_endPointUri + Path.DirectorySeparatorChar + id));
            }
        }

        public List<T> ParseList(string items)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            return JsonConvert.DeserializeObject<List<T>>(items, settings);
        }

        public T Parse(string item)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                {ContractResolver = new CustomContractResolver()};
            return JsonConvert.DeserializeObject<T>(item, settings);
        }

        public string Stringify(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        private async Task<Response<T>> HandleRequest(Func<Task<HttpResponseMessage>> httpFunction)
        {
            using (HttpResponseMessage response = await httpFunction())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.Error(nameof(HandleRequest) + "; Error; response; " + response);

                    return new Response<T>
                    {
                        StatusCode = response.StatusCode,
                        Message = "Unsuccessful",
                        Content = new T()
                    };
                }

                using (HttpContent responseContent = response.Content)
                {
                    string itemContent = await responseContent.ReadAsStringAsync();
                    _logger.Info(nameof(HandleRequest) + "; End; response; " + response + "; itemContent; " +
                                 itemContent);

                    return new Response<T>
                    {
                        StatusCode = response.StatusCode,
                        Message = "Successful",
                        Content = Parse(itemContent)
                    };
                }
            }
        }
    }
}