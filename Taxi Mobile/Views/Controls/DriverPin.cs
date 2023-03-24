namespace Taxi_mobile.Views.Controls
{
    public class DriverPin : CustomPin
    {
        public Guid DriverId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public double? Raiting { get; set; }
        public TimeSpan? Experience { get; set; }
    }
}
