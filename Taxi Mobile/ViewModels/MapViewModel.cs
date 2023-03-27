using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Windows.Input;
using Taxi_mobile.Helpers;
using Taxi_mobile.Helpers.Enams;
using Taxi_mobile.Infrastructure;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Models.Api;
using Taxi_mobile.Views.Controls;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace Taxi_mobile.ViewModels
{
    public class MapViewModel : MapPageBase
    {
        #region private_fields

        private readonly IGeolocationService _geolocationService;
        private readonly IPopupService _popupService;
        private readonly IWebService _webService;
        private readonly IProcessingService _processingService;
        private readonly IMapsApiService _mapsApiService;

        private TimeSpan _duration;
        private string _price;
        private Polyline _originDirection;
        private List<Location> _originPositions;
        private double _distanceFromUserToPoint;
        private Guid _originPinId;
        private Guid _destinationPinId;
        private Location _origin;
        private Location _destination;
        private DriverResponse _selectedDriver;
        private bool _isChooseCar;
        private bool _isRoadEnd;
        private bool _isWaiting;
        private bool _isRouteNotRunning;
        private bool _isVisibleDriverLayout;
        private bool _isVisibleSearchLayout;
        private bool _isVisibleStopRouteButton;
        private IList<MapElement> _mapElements;
        private IList<Pin> _pins;

        #endregion

        #region public_fields

        public ICommand StopRouteCommand { get; set; }
        public ICommand EnterAddressTappedCommand => new Command(async () =>
        {
            _processingService.OnCurrentStateChanged -= InitializeUi;
            await Shell.Current.GoToAsync("/SearchPlacePage");
        });
        public ICommand MapClickedCommand { get; set; }
        public ICommand MoveToRegionCommand { get; set; }
        public ICommand GetActualLocationCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand StartRideCommand { get; set; }
        public ICommand EndRoadCommand { get; set; }
        public ICommand NothingCommand => new Command(() => { });

        public TimeSpan Duration { get => _duration; set => SetProperty(ref _duration, value); }
        public string Price { get => _price; set => SetProperty(ref _price, value); }
        public double DistanceFromUserToPoint { get => _distanceFromUserToPoint; set => SetProperty(ref _distanceFromUserToPoint, value); }
        public DriverResponse SelectedDriver { get => _selectedDriver; set => SetProperty(ref _selectedDriver, value); }
        public bool IsChooseCar { get => _isChooseCar; set => SetProperty(ref _isChooseCar, value); }
        public bool IsRoadEnd { get => _isRoadEnd; set => SetProperty(ref _isRoadEnd, value); }
        public bool IsWaiting { get => _isWaiting; set => SetProperty(ref _isWaiting, value); }
        public bool IsRouteNotRunning { get => _isRouteNotRunning; set => SetProperty(ref _isRouteNotRunning, value); }
        public bool IsVisibleDriverLayout { get => _isVisibleDriverLayout; set => SetProperty(ref _isVisibleDriverLayout, value); }
        public bool IsVisibleSearchLayout { get => _isVisibleSearchLayout; set => SetProperty(ref _isVisibleSearchLayout, value); }
        public bool IsVisibleStopRouteButton { get => _isVisibleStopRouteButton; set => SetProperty(ref _isVisibleStopRouteButton, value); }
        public IList<MapElement> MapElements { get => _mapElements; set => SetProperty(ref _mapElements, value); }
        public IList<Pin> Pins { get => _pins; set => SetProperty(ref _pins, value); }

        #endregion

        public MapViewModel(IGeolocationService geolocationService, IPlatformService platformService, IPopupService popupService, IWebService webService, IProcessingService processingService, IMapsApiService mapsApiService)
            : base(platformService)
        {
            _geolocationService = geolocationService;
            _popupService = popupService;
            _webService = webService;
            _processingService = processingService;
            _mapsApiService = mapsApiService;

            StopRouteCommand = new Command(StopRoute);
            MapClickedCommand = new Command<Location>(ClickOnMap);
            AddOrderCommand = new Command(AddOrder);
            StartRideCommand = new Command(StartRide);
            EndRoadCommand = new Command(async () => await EndRoad());

            _processingService.OnCurrentStateChanged += InitializeUi;

            Initialize();
        }

        #region public

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _processingService.OnCurrentStateChanged += InitializeUi;

            if (query != null)
            {
                _originPositions = query["positions"] as List<Location>;
                _origin = query["origin"] as Location;
                _destination = query["destination"] as Location;

                CalculateRoute(_originPositions);

                DistanceFromUserToPoint = PolylineHelper.ColculateDistance(_originPositions, DistanceUnits.Kilometers);

                _processingService.SetSelectingDriverState();
            }
        }

        #endregion

        #region private

        private async void Initialize()
        {
            IsBusy = true;

            var isGpsOn = _platformService.IsGpsOn();

            if (!isGpsOn)
            {
                await _popupService.ShowInfoPopup("GPS off", "Turn on GPS");
            }

            var location = await _geolocationService.GetCurrentLocationAsync(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
            MoveToRegionCommand.Execute(mapSpan);

            await _processingService.Initialize();

            await AddDrivers();

            IsBusy = false;
        }

        private void InitializeUi(ProcessingState status)
        {
            switch (status)
            {
                case ProcessingState.NotActive:
                    {
                        IsChooseCar = false;
                        IsWaiting = false;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = true;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = true;
                        IsRoadEnd = false;

                        if (Pins is not null)
                        {
                            foreach (var driver in Pins)
                            {
                                if (driver is DriverPin)
                                {
                                    driver.MarkerClicked += OnDriverClick;
                                }
                            }
                        }
                    }
                    break;

                case ProcessingState.SelectingDriver:
                    {
                        IsChooseCar = true;
                        IsWaiting = false;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = false;

                        foreach (var driver in Pins)
                        {
                            if (driver is DriverPin)
                            {
                                driver.MarkerClicked += OnDriverClick;
                            }
                        }
                    }
                    break;

                case ProcessingState.StartOrder:
                    {
                        IsChooseCar = false;
                        IsWaiting = false;
                        IsVisibleDriverLayout = true;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = false;
                    }
                    break;

                case ProcessingState.Waiting:
                    {
                        IsChooseCar = false;
                        IsWaiting = false;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = false;

                        foreach (var driver in Pins)
                        {
                            if (driver is DriverPin)
                            {
                                driver.MarkerClicked -= OnDriverClick;
                            }
                        }
                    }
                    break;

                case ProcessingState.EndOfWaiting:
                    {
                        IsChooseCar = false;
                        IsWaiting = true;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = false;
                    }
                    break;

                case ProcessingState.Processing:
                    {
                        IsChooseCar = false;
                        IsWaiting = false;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = false;
                    }
                    break;

                case ProcessingState.EndRoad:
                    {
                        IsChooseCar = false;
                        IsWaiting = false;
                        IsVisibleDriverLayout = false;
                        IsVisibleSearchLayout = false;
                        IsVisibleStopRouteButton = false;
                        IsRouteNotRunning = false;
                        IsRoadEnd = true;
                    }
                    break;
            }
        }

        private async void AddOrder()
        {
            var addOrderRequest = new AddOrderRequest()
            {
                UserId = AppConstants.UserId,
                DriverId = SelectedDriver.Id,
                StartLatitude = _origin.Latitude,
                StartLongitude = _origin.Longitude,
                FinishLatitude = _destination.Latitude,
                FinishLongitude = _destination.Longitude,
            };

            IsBusy = true;

            await _processingService.SetWaitingState(addOrderRequest);

            var directions = await _mapsApiService.GetDirections(new Location(SelectedDriver.Latitude, SelectedDriver.Longitude), _origin);
            var positions = Enumerable.ToList(PolylineHelper.Decode(directions.Routes.First().OverviewPolyline.Points));

            var polyline = new Polyline();

            foreach (var p in positions)
            {
                polyline.Add(p);
            }

            MapElements.Remove(_originDirection);
            MapElements.Add(polyline);

            IsBusy = false;

            await LoadRoute(positions, polyline);
        }

        private void ClickOnMap(Location location)
        {
            var state = _processingService.CurrentState;

            if (state == ProcessingState.SelectingDriver || state == ProcessingState.StartOrder)
            {
                MapElements.Clear();

                var cPin = Pins.Where(p => p is CustomPin)
                           .Cast<CustomPin>()
                           .Where(p => p.Id == _originPinId || p.Id == _destinationPinId)
                           .ToArray();


                Pins.Remove(cPin[0]);
                Pins.Remove(cPin[1]);

                _processingService.SetNotActiveState();
            }
        }

        private async void StartRide()
        {
            IsBusy = true;

            await _processingService.SetProcessingState();

            var cPin = Pins.Where(p => p is DriverPin)
                           .Cast<DriverPin>()
                           .FirstOrDefault(p => p.DriverId == _selectedDriver.Id);

            Pins.Clear();
            Pins.Add(cPin);

            MapElements.Clear();
            MapElements.Add(_originDirection);

            IsBusy = false;

            await LoadRoute(_originPositions, _originDirection);
        }

        private async Task EndRoad()
        {
            IsBusy = true;
            
            var request = new FinishCarRequest()
            {
                Price = decimal.Parse(Price),
                Duration = Duration.Ticks,
                Distance = DistanceFromUserToPoint
            };

            await _processingService.SetNotActiveState(request);

            await AddDrivers();

            IsBusy = false;

            var location = await _geolocationService.GetCurrentLocationAsync(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
            MoveToRegionCommand.Execute(mapSpan);
        }

        private void OnDriverClick(object sender, PinClickedEventArgs e)
        {
            var pin = sender as DriverPin;

            var driver = new DriverResponse()
            {
                Id = pin.DriverId,
                Name = pin.Name,
                Surname = pin.Surname,
                Raiting = pin.Raiting,
                Experience = pin.Experience,
                Latitude = pin.Location.Latitude,
                Longitude = pin.Location.Longitude,
            };

            SelectedDriver = driver;

            _processingService.SetStartOrderState();
        }

        private async Task LoadRoute(List<Location> positions, Polyline polyline)
        {
            var positionIndex = 1;

            var dateTimeOfStart = DateTime.Now;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (positions.Count > positionIndex)
                {
                    Duration.Add(TimeSpan.FromSeconds(1));

                    UpdatePosition(positions[positionIndex], polyline);
                    positionIndex++;

                    return true;
                }
                else
                {
                    var task = _processingService.CurrentState switch
                    {
                        ProcessingState.Waiting => _processingService.SetEndOfWaiting(),
                        ProcessingState.Processing => _processingService.SetEndRoadState()
                    };

                    task.Wait();

                    Duration = DateTime.Now - dateTimeOfStart;

                    return false;
                }
            });
        }

        private async void UpdatePosition(Location position, Polyline polyline)
        {
            if (Pins.Count == 1 && MapElements != null && MapElements?.Count > 1)
                return;

            var cPin = Pins.Where(p => p is DriverPin).Cast<DriverPin>().FirstOrDefault(p => p.DriverId == SelectedDriver.Id);

            if (cPin != null)
            {
                var newPin = new DriverPin() {
                    Location = new Location(position.Latitude, position.Longitude),
                    DriverId = cPin.DriverId,
                    Raiting = cPin.Raiting,
                    Name = cPin.Name,
                    Surname = cPin.Surname,
                    Experience = cPin.Experience,
                    ImageSource = ImageSource.FromFile("car_pin.svg"),
                    Type = PinType.Place,
                    Label = "Driver",
                    Address = "Driver",
                };

                Pins.Remove(cPin);
                Pins.Add(newPin);

                var mapSpan = MapSpan.FromCenterAndRadius(new Location(position.Latitude, position.Longitude), Distance.FromMeters(200));
                MoveToRegionCommand.Execute(mapSpan);

                polyline.Remove(polyline.FirstOrDefault());
            }
            else
            {
                MapElements?.Clear();
            }
        }

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

            _originDirection = polyline;

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

            _originPinId = pin.Id;

            var pin1 = new CustomPin
            {
                Type = PinType.Place,
                Location = new Location(polyline.Last().Latitude, polyline.Last().Longitude),
                Label = "Last",
                Address = "Last",
                ImageSource = ImageSource.FromFile("position_pin.svg")
            };
            
            Pins.Add(pin1);

            _destinationPinId = pin1.Id;
        }

        private void StopRoute()
        {
            IsVisibleSearchLayout = true;
            IsVisibleStopRouteButton = false;

            MapElements.Clear();
            Pins.Clear();
        }

        private async Task AddDrivers()
        {
            Pins.Clear();

            var drivers = await _webService.GetAllDrivers("Free");

            foreach (var driver in drivers.Drivers)
            {
                Pins.Add(new DriverPin()
                {
                    Label = "Driver",
                    Type = PinType.Place,
                    ImageSource = ImageSource.FromFile("car_pin.svg"),
                    Location = new Location(driver.Latitude, driver.Longitude),
                    DriverId = driver.Id,
                    Name = driver.Name,
                    Surname = driver.Surname,
                    Raiting = driver.Raiting,
                    Experience = driver.Experience,
                });
            }
        }

        #endregion
    }
}
