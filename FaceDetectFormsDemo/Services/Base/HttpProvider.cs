using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceDetectFormsDemo.Services
{
    public class HttpProvider: IHttpProvider
    {
        readonly HttpClient _httpClient;
        public HttpProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task<TResult> PostAsync<TResult>(string endpoint,
                                                      object data,
                                                      params KeyValuePair<string, string>[] headers)
        {


            AddDefaultRequestHeaders(headers);

            var content = data != null 
                            ? data is byte[] ? new ByteArrayContent(data as byte[]): new StringContent(JsonConvert.SerializeObject(data)) 
                            : null;

            if (data is byte[] && content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else if(content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<TResult>(response);


        }


        public async Task DeleteAsync(string endpoint,
                                      params KeyValuePair<string,string>[] headers)
        {

            AddDefaultRequestHeaders(headers);

            await _httpClient.DeleteAsync(endpoint);

        }

        public async Task<TResult> PutAsync<TResult>(string endpoint, 
                                           object data,
                                           params KeyValuePair<string, string>[] headers)
        {
            AddDefaultRequestHeaders(headers);

            var content = data != null
                            ? data is byte[] ? new ByteArrayContent(data as byte[]) : new StringContent(JsonConvert.SerializeObject(data))
                            : null;

            if (data is byte[] && content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            else if (content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var response = await _httpClient.PutAsync(endpoint, content);

            return await HandleResponse<TResult>(response);
        }

        public async Task<TResult> GetAsync<TResult>(string endpoint,
                                                     params KeyValuePair<string, string>[] headers)
        {
            AddDefaultRequestHeaders(headers);
            var response = await _httpClient.GetAsync(endpoint);

            return await HandleResponse<TResult>(response);

        }

        public async Task<TResult> PatchAsync<TResult>(string endpoint,
                                                              object data,
                                                              params KeyValuePair<string, string>[] headers)
        {
            AddDefaultRequestHeaders(headers);
            var method = new HttpMethod("PATCH");
            var content = data != null ? new StringContent(JsonConvert.SerializeObject(data)) : null;
            if (content != null)
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = content
            };

            HttpResponseMessage response = new HttpResponseMessage();

            response = await _httpClient.SendAsync(request);

            return await HandleResponse<TResult>(response);

        }

        void AddDefaultRequestHeaders(params KeyValuePair<string, string>[] headers)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        async Task<TResult> HandleResponse<TResult>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                Debug.WriteLine(json);
                var res = JsonConvert.DeserializeObject<TResult>(json);
                return res;
            }
            else
            {
                throw new AzureException();
            }
        }
    }

    public class AzureException : Exception
    {
        public  AzureException()
        {
        }

    }

    public class EmptyClass
    {

    }
}
