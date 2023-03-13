using Android.Content;
using Android.Locations;
using Taxi_mobile.Interfaces.Platforms;

namespace Taxi_mobile.Services.Platforms
{
    public partial class PlatformService : IPlatformService
    {
        public partial bool IsGpsOn()
        {
            var locManager = (LocationManager)MauiApplication.Context.GetSystemService(Context.LocationService);

            return locManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
    }
}
