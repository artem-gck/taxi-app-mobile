using Taxi_mobile.Models.GoogleMaps;

namespace Taxi_mobile.Interfaces.Platforms
{
    public interface IMapsApiService
    {
        public Task<GoogleDirection> GetDirections(Location origin, Location destination);
        public Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        public Task<GooglePlace> GetPlaceDetails(string placeId);
    }
}
