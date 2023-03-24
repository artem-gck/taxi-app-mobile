namespace Taxi_mobile.Models.Api
{
    public class GetAllDriversResponse
    {
        public List<DriverResponse> Drivers { get; set; }
    }

    public class DriverResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public double? Raiting { get; set; }
        public TimeSpan? Experience { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
