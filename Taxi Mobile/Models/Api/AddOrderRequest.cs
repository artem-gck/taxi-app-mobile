namespace Taxi_mobile.Models.Api
{
    public class AddOrderRequest
    {
        public Guid UserId { get; set; }
        public Guid DriverId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double FinishLatitude { get; set; }
        public double FinishLongitude { get; set; }
    }
}
