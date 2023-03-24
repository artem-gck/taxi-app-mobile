using Android.Gms.Maps;
using Microsoft.Maui.Maps.Handlers;
using IMap = Microsoft.Maui.Maps.IMap;

namespace Taxi_mobile.Views.Handlers
{
    public class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly IMapHandler mapHandler;

        public MapCallbackHandler(IMapHandler mapHandler)
        {
            this.mapHandler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {

            if (googleMap is not null)
            {
#if DEBUG
                googleMap.UiSettings.ZoomControlsEnabled = true;
#else
                googleMap.UiSettings.ZoomControlsEnabled = false;
#endif
            }

            mapHandler.UpdateValue(nameof(IMap.Pins));
        }
    }
}
