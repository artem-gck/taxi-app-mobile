using System.Windows.Input;
using Taxi_mobile.Infrastructure;
using Microsoft.Maui.Controls.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Microsoft.Maui.Maps;

namespace Taxi_mobile.ViewModels
{
    public class MapPageViewModel : MapPageBase
    {
        public Map Map { get; private set; }
        private Random random;
        public ICommand GetCurrentLocationCommand { get; set; }

        public MapPageViewModel(IGeolocation geolocation, IGeocoding geocoding)
            : base()
        {
            Map = new Map();
            Map.IsShowingUser = true;
            Map.MapClicked += OnMapClicked;



            _geolocation = geolocation;
            _geocoding = geocoding;

            random = new Random(Guid.NewGuid().GetHashCode());

            GetCurrentLocationCommand = new Command(async () => await GetCurrentLocationAsync());

            Initialize();
        }

        private async void Initialize()
        {
            IsBusy = true;
            cts = new CancellationTokenSource();


            var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));
            var location = await _geolocation.GetLocationAsync(request, cts.Token);
           
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
            Map.MoveToRegion(mapSpan);
            IsBusy = false;

        }

        private CancellationTokenSource cts;
        private IGeolocation _geolocation;
        private IGeocoding _geocoding;

        private async Task GetCurrentLocationAsync()
        {
            try
            {
                IsBusy = true;

                cts = new CancellationTokenSource();

                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                //var location = await _geolocation.GetLocationAsync(request, cts.Token);

                string address = "Минск";
                var locations = await _geocoding.GetLocationsAsync(address);

                foreach (var loc in locations)
                {
                    Pin pin = new Pin
                    {
                        Label = "Santa Cruz",
                        Address = "The city with a boardwalk",
                        Type = PinType.SearchResult,
                        Location = loc
                    };

                    Map.Pins.Add(pin);
                }

                MapSpan mapSpan = MapSpan.FromCenterAndRadius(locations.First(), Distance.FromKilometers(100));
                Map.MoveToRegion(mapSpan);

                //var a = random.NextDouble() * 100;
                //var b = random.NextDouble() * 100;

                //var location = new Location(a, b);

                //location.Longitude = a;
                //location.Latitude = b;

                ////Map.Pins.Clear();

                //MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
                //Map.MoveToRegion(mapSpan);

                //Pin pin = new Pin
                //{
                //    Label = "Santa Cruz",
                //    Address = "The city with a boardwalk",
                //    Type = PinType.SearchResult,
                //    Location = location
                //};

                //Map.Pins.Add(pin);
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            IsBusy = false;
        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");

            Pin pin = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.SearchResult,
                Location = e.Location
            };

            Map.Pins.Add(pin);
        }
    }
}
