namespace Taxi_mobile.Helpers
{
    public static class ApiRouteGenerator
    {
        public static class DriversService
        {
            public static string GetAllDriversPath(string status)
                => $"{AppConstants.TaxiApiRoute}/api/drivers?status={status}";

            public static string GetUserStatePath(Guid id)
                => $"{AppConstants.TaxiApiRoute}/api/users/{id}";

            public static string GetAddOrderPath()
                => $"{AppConstants.TaxiApiRoute}/api/orders";

            public static string GetProcessingOrderPath(Guid orderId)
                => $"{AppConstants.TaxiApiRoute}/api/orders/process/{orderId}";

            public static string GetFinishOrderPath(Guid orderId)
                => $"{AppConstants.TaxiApiRoute}/api/orders/delete/{orderId}";
        }
    }
}
