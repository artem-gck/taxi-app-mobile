namespace Taxi_mobile.Helpers
{
    public static class AppConstants
    {
#if DEBUG

        public static string TaxiApiRoute => "http://10.0.2.2:8083";
        public static string GoogleMapsRoute => "https://maps.googleapis.com/maps";

#else

        public static string TaxiApiRoute => "http://10.0.2.2:8083";
        public static string GoogleMapsRoute => "https://maps.googleapis.com/maps";

#endif

        public static Guid UserId => Guid.Parse("f1b6756c-c8e1-417e-8b87-f36b6b528a92");
        public static int CountOfRecentPlaces => 5;
    }
}
