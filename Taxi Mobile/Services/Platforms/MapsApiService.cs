using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Models;

namespace Taxi_mobile.Services.Platforms
{
    public partial class MapsApiService : IMapsApiService
    {
        public partial Task<GoogleDirection> GetDirections(Location origin, Location destination);
        public partial Task<GooglePlace> GetPlaceDetails(string placeId);
        public partial Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
    }
}
