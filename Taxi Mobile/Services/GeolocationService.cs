using Microsoft.Maui.Devices.Sensors;
using Taxi_mobile.Interfaces;

namespace Taxi_mobile.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IGeolocation _geolocation;

        public GeolocationService(IGeolocation geolocation) 
        {
            _geolocation = geolocation;
        }

        public async Task<Location> GetCurrentLocationAsync(GeolocationAccuracy accuracy, TimeSpan timeout)
        {
            var cts = new CancellationTokenSource();
            var request = new GeolocationRequest(accuracy, timeout);
            
            return await _geolocation.GetLocationAsync(request, cts.Token);
        }
    }
}
