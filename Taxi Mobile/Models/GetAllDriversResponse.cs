namespace Taxi_mobile.Models
{
    public class GetAllDriversResponse
    {
        public List<DriverResponse> Drivers { get; set; }
    }

    public class DriverResponse
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
