using Taxi_mobile.Models;

namespace Taxi_mobile.Interfaces.Platforms
{
    public interface IMapsApiService
    {
        public Task<GoogleDirection> GetDirections(Location origin, Location destination);
        public Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        public Task<GooglePlace> GetPlaceDetails(string placeId);
    }
}
