namespace Taxi_mobile.Interfaces
{
    public interface IGeolocationService
    {
        public Task<Location> GetCurrentLocation(GeolocationAccuracy accuracy, TimeSpan timeout);
    }
}
