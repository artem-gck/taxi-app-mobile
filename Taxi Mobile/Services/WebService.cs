using Newtonsoft.Json;
using System.Net.Http.Headers;
using Taxi_mobile.Helpers;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Models;

namespace Taxi_mobile.Services
{
    public class WebService : IWebService
    {
        private const HttpMethodType GET = HttpMethodType.Get;
        private const HttpMethodType PUT = HttpMethodType.Put;
        private const HttpMethodType POST = HttpMethodType.Post;
        private const HttpMethodType DELETE = HttpMethodType.Delete;

        private enum HttpMethodType
        {
            Get,
            Put,
            Post,
            Delete
        }

        public async Task<GetAllDriversResponse> GetAllDrivers(string status)
        {
            var url = ApiRouteGenerator.DriversService.GetAllDriversPath(status);
            return await CallApiJson<GetAllDriversResponse>(url);
        }

        private async Task<T> CallApiJson<T>(string url, HttpMethodType httpMethod = HttpMethodType.Get, StringContent content = null)
        {
            var responseMessage = await HttpSend(url, httpMethod, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<T>(jsonString);

                return response;
            }

            throw new Exception();
        }

        private async Task<HttpResponseMessage> HttpSend(string url, HttpMethodType method, StringContent content = null)
        {
            using var httpClient = new HttpClient();

            if (Device.RuntimePlatform == Device.Android)
            {
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
            }

            httpClient.Timeout = TimeSpan.FromSeconds(100);

            return method switch
            {
                GET => await httpClient.GetAsync(url),
                PUT => await httpClient.PutAsync(url, content),
                POST => await httpClient.PostAsync(url, content),
                DELETE => await httpClient.DeleteAsync(url),
                _ => throw new ArgumentException($"HttpMethodType:{method} is not a valid Type")
            };
        }
    }
}
