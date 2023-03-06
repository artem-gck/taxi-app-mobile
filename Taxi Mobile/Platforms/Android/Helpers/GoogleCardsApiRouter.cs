namespace Taxi_mobile.Platforms.Android.Helpers
{
    public static class GoogleCardsApiRouter
    {
        public static string ApiBaseAddress => "https://maps.googleapis.com/maps/";
        private static string GoogleMapsApiKey => MetadataHelper.GetMapsApiKey();

        public static string GetDirectionsPath(Location original, Location destination)
            => $"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={original.Latitude},{original.Longitude}&destination={destination.Latitude},{destination.Longitude}&key={GoogleMapsApiKey}";

        public static string GetPlacesPath(string text)
            => $"api/place/autocomplete/json?input={Uri.EscapeDataString(text)}&key={GoogleMapsApiKey}";

        public static string GetPlaceDetailsPath(string placeId)
            => $"api/place/details/json?placeid={Uri.EscapeDataString(placeId)}&key={GoogleMapsApiKey}";
    }
}
