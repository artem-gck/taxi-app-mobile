﻿namespace Taxi_mobile.Helpers
{
    public static class AppConstants
    {
        public static string TaxiApiRoute { get => "http://10.0.2.2:8083"; }

        public static string GoogleMapsRoute { get => "https://maps.googleapis.com/maps"; }

        public static Guid UserId { get => Guid.Parse("f1b6756c-c8e1-417e-8b87-f36b6b528a92"); }
        public static int CountOfRecentPlaces => 5;
    }
}
