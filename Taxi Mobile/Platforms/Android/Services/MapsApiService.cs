using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Taxi_mobile.Models;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Platforms.Android.Helpers;

namespace Taxi_mobile.Services.Platforms
{
    public partial class MapsApiService : IMapsApiService
    {
        private readonly HttpClient _httpClient;

        public MapsApiService()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public partial async Task<GoogleDirection> GetDirections(Location origin, Location destination)
        {
            var path = GoogleCardsApiRouter.GetDirectionsPath(origin, destination);
            var response = await _httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<GoogleDirection>(json);
                }
            }
        
            return new GoogleDirection();
        }

        public partial async Task<GooglePlaceAutoCompleteResult> GetPlaces(string text)
        {
            var path = GoogleCardsApiRouter.GetPlacesPath(text);

            var response = await _httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                {
                    return JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(json);
                }
            }

            return null;
        }

        public partial async Task<GooglePlace> GetPlaceDetails(string placeId)
        {
            var path = GoogleCardsApiRouter.GetPlaceDetailsPath(placeId);
            var response = await _httpClient.GetAsync(path);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                {
                    return new GooglePlace(JObject.Parse(json));
                }
            }

            return null;
        }
    }
}
