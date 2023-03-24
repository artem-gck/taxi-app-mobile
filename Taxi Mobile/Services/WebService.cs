using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Taxi_mobile.Helpers;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Models.Api;

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

        public async Task<GetUserStateResponse> GetUserState(Guid id)
        {
            var url = ApiRouteGenerator.DriversService.GetUserStatePath(id);
            return await CallApiJson<GetUserStateResponse>(url);
        }

        public async Task<AddOrderResponse> PostAddOrder(AddOrderRequest orderRequest)
        {
            var url = ApiRouteGenerator.DriversService.GetAddOrderPath();
            var content = JsonContent(JsonConvert.SerializeObject(orderRequest));
            return await CallApiJson<AddOrderResponse>(url, HttpMethodType.Post, content);
        }

        public async Task<ProcessCarResponse> PutProcessOrder(Guid orderId)
        {
            var url = ApiRouteGenerator.DriversService.GetProcessingOrderPath(orderId);
            return await CallApiJson<ProcessCarResponse>(url, HttpMethodType.Put);
        }

        public async Task<FinishCarResponse> PutFinishOrder(Guid orderId, FinishCarRequest orderRequest)
        {
            var url = ApiRouteGenerator.DriversService.GetFinishOrderPath(orderId);
            var content = JsonContent(JsonConvert.SerializeObject(orderRequest));
            return await CallApiJson<FinishCarResponse>(url, HttpMethodType.Put, content);
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

        private StringContent JsonContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
