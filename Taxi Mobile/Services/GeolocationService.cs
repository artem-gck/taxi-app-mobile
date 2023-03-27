using Taxi_mobile.Interfaces;

namespace Taxi_mobile.Services
{
    public class GeolocationService : IGeolocationService
    {
        #region private_fields

        private readonly IGeolocation _geolocation;

        #endregion

        public GeolocationService(IGeolocation geolocation) 
        {
            _geolocation = geolocation;
        }

        #region public

        public async Task<Location> GetCurrentLocationAsync(GeolocationAccuracy accuracy, TimeSpan timeout)
        {
            var cts = new CancellationTokenSource();
            var request = new GeolocationRequest(accuracy, timeout);
            
            return await _geolocation.GetLocationAsync(request, cts.Token);
        }

        #endregion
    }
}
