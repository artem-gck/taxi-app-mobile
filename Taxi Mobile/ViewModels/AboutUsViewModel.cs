using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Taxi_mobile.Helpers.Enams;
using Taxi_mobile.Infrastructure;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Views.Controls;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace Taxi_mobile.ViewModels
{
    public class AboutUsViewModel : MapPageBase
    {
        public ICommand StopRouteCommand { get; set; }
        public ICommand EnterAddressTappedCommand => new Command(async () =>
        {
            _processingService.OnCurrentStateChanged -= Initialize;
            await Shell.Current.GoToAsync("/SearchPlacePage");
        });
    
        public ICommand MapClickedCommand { get; set; }
        public ICommand MoveToRegionCommand { get; set; }
        public ICommand GetActualLocationCommand { get; set; }

        public bool IsRouteNotRunning { get => _isRouteNotRunning; set => SetProperty(ref _isRouteNotRunning, value); }
        public bool IsVisibleDriverLayout { get => _isVisibleDriverLayout; set => SetProperty(ref _isVisibleDriverLayout, value); }
        public bool IsVisibleSearchLayout { get => _isVisibleSearchLayout; set => SetProperty(ref _isVisibleSearchLayout, value); }
        public bool IsVisibleStopRouteButton { get => _isVisibleStopRouteButton; set => SetProperty(ref _isVisibleStopRouteButton, value); }
        public IList<MapElement> MapElements { get => _mapElements; set => SetProperty(ref _mapElements, value); }
        public IList<Pin> Pins { get => _pins; set => SetProperty(ref _pins, value); }

        private readonly IGeolocationService _geolocationService;
        private readonly IPopupService _popupService;
        private readonly IWebService _webService;
        private readonly IProcessingService _processingService;

        private bool _isRouteNotRunning;
        private bool _isVisibleDriverLayout;
        private bool _isVisibleSearchLayout;
        private bool _isVisibleStopRouteButton;
        private IList<MapElement> _mapElements;
        private IList<Pin> _pins;
        

        public AboutUsViewModel(IGeolocationService geolocationService, IPlatformService platformService, IPopupService popupService, IWebService webService, IProcessingService processingService)
            : base(platformService)
        {
            _geolocationService = geolocationService;
            _popupService = popupService;
            _webService = webService;
            _processingService = processingService;
            
            StopRouteCommand = new Command(StopRoute);
            MapClickedCommand = new Command<Location>(Click);

            _processingService.OnCurrentStateChanged += Initialize;
            _processingService.SetNotActiveState();

            Initialize();
        }

        private async void Initialize()
        {
            var isGpsOn = _platformService.IsGpsOn();

            if (!isGpsOn)
            {
                await _popupService.ShowInfoPopup("GPS off", "Turn on GPS");
            }

            var location = await _geolocationService.GetCurrentLocationAsync(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
            MoveToRegionCommand.Execute(mapSpan);

            await AddRandomRiders();
        }

        private async Task Initialize(ProcessingState status)
        {
            switch (status)
            {
                case ProcessingState.NotActive:
                    {
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = true;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = true;
                    }
                    break;

                case ProcessingState.SelectingDriver:
                    {
                        IsVisibleDriverLayout = true;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                    }
                    break;
            }
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _processingService.OnCurrentStateChanged += Initialize;

            if (query != null)
            {
                var locations = query["positions"] as List<Location>;

                CalculateRoute(locations);

                IsRouteNotRunning = true;

                foreach (var driver in Pins)
                {
                    if (driver is DriverPin)
                    {
                        driver.MarkerClicked += OnDriverClick;
                    }
                }
            }
        }


        private async Task AddRandomRiders()
        {
            var drivers = await _webService.GetAllDrivers("Free");

            var driverPins = new List<DriverPin>();

            driverPins.Add(new DriverPin()
            {
                Label = "Driver",
                Type = PinType.Place,
                DriverId = drivers.Drivers[0].Id,
                ImageSource = ImageSource.FromFile("car_pin.svg"),
                Location = new Location(drivers.Drivers[0].Latitude, drivers.Drivers[0].Longitude)
            });

            driverPins.Add(new DriverPin()
            {
                Label = "Driver",
                Type = PinType.Place,
                DriverId = drivers.Drivers[1].Id,
                ImageSource = ImageSource.FromFile("car_pin.svg"),
                Location = new Location(drivers.Drivers[1].Latitude, drivers.Drivers[1].Longitude)
            });

            driverPins.Add(new DriverPin()
            {
                Label = "Driver",
                Type = PinType.Place,
                DriverId = drivers.Drivers[2].Id,
                ImageSource = ImageSource.FromFile("car_pin.svg"),
                Location = new Location(drivers.Drivers[2].Latitude, drivers.Drivers[2].Longitude)
            });

            foreach (var driver in driverPins)
                Pins.Add(driver);
        }

        private void OnDriverClick(object sender, PinClickedEventArgs e)
        {
            var send = sender as DriverPin;

            _processingService.SetSelectingDriverState();
        }

        private void Click(Location location)
        {
            var a = location;

            if (_isVisibleDriverLayout)
            {
                _processingService.SetNotActiveState();
            }
        }

        //public async Task LoadRoute()
        //{
        //    var positionIndex = 1;
        //    var googleDirection = await _mapsApiService.GetDirections(_origin, _destination);
        //    if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
        //    {
        //        var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
        //        //Map.CalculateCommand.Execute(positions);
        //                                //CalculateRouteCommand.Execute(positions);
        //                                CalculateRoute(positions);
        //        _hasRouteRunning = true;

        //        //Location tracking simulation
        //        //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        //        //{
        //        //    if (positions.Count > positionIndex && _hasRouteRunning)
        //        //    {
        //        //        UpdatePosition(positions[positionIndex]);
        //        //        //UpdatePositionCommand.Execute(positions[positionIndex]);
        //        //        positionIndex++;
        //        //        return true;
        //        //    }
        //        //    else
        //        //    {
        //        //        return false;
        //        //    }
        //        //});
        //    }
        //    else
        //    {
        //        await _alertService.DisplayAlert(":(", "No route found", "Ok");
        //    }

        //}

        //private async void UpdatePosition(Location position)
        //{
        //    if (Pins.Count == 1 && MapElements != null && MapElements?.Count > 1)
        //        return;

        //    var cPin = Pins.FirstOrDefault();

        //    if (cPin != null)
        //    {
        //        cPin.Location = new Location(position.Latitude, position.Longitude);

        //        //cPin.Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_taxi.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 25, HeightRequest = 25 });
        //        var mapSpan = MapSpan.FromCenterAndRadius(new Location(position.Latitude, position.Longitude), Distance.FromKilometers(5));
        //        MoveToRegionCommand.Execute(mapSpan);
        //        //var previousPosition = MapElements?.FirstOrDefault();
        //        //Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
        //    }
        //    else
        //    {
        //        //END TRIP
        //        MapElements?.Clear();
        //    }


        //}

        private void CalculateRoute(List<Location> positions)
        {
            IsVisibleSearchLayout = false;
            IsVisibleStopRouteButton = true;

            MapElements.Clear();

            var polyline = new Polyline();

            foreach (var p in positions)
            {
                polyline.Add(p);
            }

            MapElements.Add(polyline);

            MoveToRegionCommand.Execute(MapSpan.FromCenterAndRadius(new Location(polyline.First().Latitude, polyline.First().Longitude), Distance.FromMeters(200)));

            var pin = new CustomPin
            {
                Type = PinType.Place,
                Location = new Location(polyline.First().Latitude, polyline.First().Longitude),
                Label = "First",
                Address = "First",
                ImageSource = ImageSource.FromFile("position_pin.svg")
            };

            Pins.Add(pin);
            
            var pin1 = new CustomPin
            {
                Type = PinType.Place,
                Location = new Location(polyline.Last().Latitude, polyline.Last().Longitude),
                Label = "Last",
                Address = "Last",
                ImageSource = ImageSource.FromFile("position_pin.svg")
            };
            
            Pins.Add(pin1);
        }

        public void StopRoute()
        {
            IsVisibleSearchLayout = true;
            IsVisibleStopRouteButton = false;

            MapElements.Clear();
            Pins.Clear();

            //_hasRouteRunning = false;
        }
    }
}
