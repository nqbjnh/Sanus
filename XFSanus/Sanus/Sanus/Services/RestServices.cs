using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sanus.Services
{
    public class RestServices
    {
        readonly HttpClient client;

        public RestServices()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = Configuration.MAXRESPONSECONTENTBUFFERSIZE
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Configuration.CONTENTYPE));
        }

        public async Task<T> GetResponse<T>(string api) where T : class
        {
            var response = await client.GetStringAsync(Configuration.CoreApi(api));
            var result = JsonConvert.DeserializeObject<T>(response, Configuration.JsonSettings);
            return result;
        }

        public async Task<T> PostResponse<T>(string webUrl, object obj) where T : class
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None, Configuration.JsonSettings);
            var response = await client.PostAsync(
                Configuration.CoreApi(webUrl),
                new StringContent(json, Encoding.UTF8, Configuration.CONTENTYPE));
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            if (jsonResult.Equals(Configuration.PERMISSION))
            {
                return null;
            }
            else
            {
                var result = JsonConvert.DeserializeObject<T>(jsonResult);
                return result;
            }
        }

        public async Task<int> PostResponse(string webUrl, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None, Configuration.JsonSettings);
            var response = await client.PostAsync(
                Configuration.CoreApi(webUrl),
                new StringContent(json, Encoding.UTF8, Configuration.CONTENTYPE));
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            if (jsonResult.Equals(Configuration.PERMISSION))
            {
                return 0;
            }
            else
            {
                int result = int.Parse(jsonResult.ToString());
                return result;
            }
        }
    }
}
