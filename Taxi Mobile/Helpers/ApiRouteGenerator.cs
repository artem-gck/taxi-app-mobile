namespace Taxi_mobile.Helpers
{
    public static class ApiRouteGenerator
    {
        public static class DriversService
        {
            public static string GetAllDriversPath(string status)
                => $"{AppConstants.TaxiApiRoute}/api/drivers?status={status}";
        }
    }
}
