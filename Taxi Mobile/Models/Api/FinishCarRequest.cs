namespace Taxi_mobile.Models.Api
{
    public class FinishCarRequest
    {
        public Guid OrderId { get; set; }
        public decimal? Price { get; set; }
        public double? Duration { get; set; }
        public double? Distance { get; set; }
    }
}
