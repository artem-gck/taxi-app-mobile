namespace Taxi_mobile.Interfaces
{
    public interface IGeolocationService
    {
        public Task<Location> GetCurrentLocationAsync(GeolocationAccuracy accuracy, TimeSpan timeout);
    }
}
