using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Taxi_mobile.Helpers;
using Taxi_mobile.Infrastructure;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Models;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace Taxi_mobile.ViewModels
{
    public class AboutUsViewModel : MapPageBase
    {
        public ICommand LoadRouteCommand { get; set; }
        public ICommand StopRouteCommand { get; set; }
        public ICommand EnterAddressTappedCommand => new Command(async () => await Shell.Current.GoToAsync("/SearchPlacePage"));
        public ICommand MapClickedCommand { get; set; }
        public ICommand MoveToRegionCommand { get; set; }

        public ICommand FocusOriginCommand { get; set; }

        public ICommand GetPlacesCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }
        public ICommand GetActualLocationCommand { get; set; }





        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> RecentPlaces { get; set; } = new ObservableCollection<GooglePlaceAutoCompletePrediction>();
        public bool IsShowRecentPlaces { get => _isShowRecentPlaces; set => SetProperty(ref _isShowRecentPlaces, value); }
        private bool _isShowRecentPlaces;
        public GooglePlaceAutoCompletePrediction PlaceSelected
        {
            get => _placeSelected;
            set => SetProperty(ref _placeSelected, value, () =>
            {
                if (_placeSelected != null)
                    GetPlaceDetailCommand.Execute(_placeSelected);
            });
        }
        public string PickupText
        {
            get => _pickupText;
            set => SetProperty(ref _pickupText, value, () => {
                if (!string.IsNullOrEmpty(_pickupText))
                {
                    _isPickupFocused = true;
                    GetPlacesCommand.Execute(_pickupText);
                }
            });
        }
        public string OriginText
        {
            get => _originText;
            set => SetProperty(ref _originText, value, () =>
            {
                if (!string.IsNullOrEmpty(_originText))
                {
                    _isPickupFocused = false;
                    GetPlacesCommand.Execute(_originText);
                }
            });
        }
        public bool IsVisibleSearchLayout { get => _isVisibleSearchLayout; set => SetProperty(ref _isVisibleSearchLayout, value); }
        public bool IsVisibleStopRouteButton { get => _isVisibleStopRouteButton; set => SetProperty(ref _isVisibleStopRouteButton, value); }


        private readonly IMapsApiService _mapsApiService;
        private readonly IAlertService _alertService;
        private readonly IGeolocationService _geolocationService;

        private bool _hasRouteRunning;
        private Location _origin;
        private Location _destination;
        private GooglePlaceAutoCompletePrediction _placeSelected;
        private bool _isPickupFocused = true;
        private string _pickupText;
        private string _originText;

        private bool _isVisibleSearchLayout = true;
        private bool _isVisibleStopRouteButton = false;




        private IList<MapElement> _mapElements;
        public IList<MapElement> MapElements { get => _mapElements; set => SetProperty(ref _mapElements, value); }




        private IList<Pin> _pins;
        public IList<Pin> Pins { get => _pins; set => SetProperty(ref _pins, value); }

        public AboutUsViewModel(IMapsApiService mapsApiService, IAlertService alertService, IGeolocationService geolocationService)
        {
            _mapsApiService = mapsApiService;
            _alertService = alertService;
            _geolocationService = geolocationService;

            LoadRouteCommand = new Command(async () => await LoadRoute());
            StopRouteCommand = new Command(StopRoute);
            GetPlacesCommand = new Command<string>(async (param) => await GetPlacesByName(param));
            GetPlaceDetailCommand = new Command<GooglePlaceAutoCompletePrediction>(async (param) => await GetPlacesDetail(param));

            MapClickedCommand = new Command<Location>(Click);

            Places = new ObservableCollection<GooglePlaceAutoCompletePrediction>();

            Initialize();
        }

        private void Click(Location location)
        {
            var a = 0;
        }

        private async void Initialize()
        {
            await CalculateRoute();
        }

        private async Task CalculateRoute()
        {
            var location = await _geolocationService.GetCurrentLocation(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));
            MoveToRegionCommand.Execute(mapSpan);
        }

        public async Task LoadRoute()
        {
            var positionIndex = 1;
            var googleDirection = await _mapsApiService.GetDirections(_origin, _destination);
            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {
                var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                //Map.CalculateCommand.Execute(positions);
                                        //CalculateRouteCommand.Execute(positions);
                                        CalculateRoute(positions);
                _hasRouteRunning = true;

                //Location tracking simulation
                //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                //{
                //    if (positions.Count > positionIndex && _hasRouteRunning)
                //    {
                //        UpdatePosition(positions[positionIndex]);
                //        //UpdatePositionCommand.Execute(positions[positionIndex]);
                //        positionIndex++;
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //});
            }
            else
            {
                await _alertService.DisplayAlert(":(", "No route found", "Ok");
            }

        }

        async void UpdatePosition(Location position)
        {
            if (Pins.Count == 1 && MapElements != null && MapElements?.Count > 1)
                return;

            var cPin = Pins.FirstOrDefault();

            if (cPin != null)
            {
                cPin.Location = new Location(position.Latitude, position.Longitude);

                //cPin.Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_taxi.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 25, HeightRequest = 25 });
                var mapSpan = MapSpan.FromCenterAndRadius(new Location(position.Latitude, position.Longitude), Distance.FromKilometers(5));
                MoveToRegionCommand.Execute(mapSpan);
                //var previousPosition = MapElements?.FirstOrDefault();
                //Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
            }
            else
            {
                //END TRIP
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

            MapElements.Add(polyline);

            MoveToRegionCommand.Execute(MapSpan.FromCenterAndRadius(new Location(polyline.First().Latitude, polyline.First().Longitude), Distance.FromMeters(200)));

            var pin = new Pin
            {
                Type = PinType.Place,
                Location = new Location(polyline.First().Latitude, polyline.First().Longitude),
                Label = "First",
                Address = "First",
                //Tag = string.Empty,
                //Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_taxi.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 25, HeightRequest = 25 })

            };
            Pins.Add(pin);
            var pin1 = new Pin
            {
                Type = PinType.Place,
                Location = new Location(polyline.Last().Latitude, polyline.Last().Longitude),
                Label = "Last",
                Address = "Last",

                //Tag = string.Empty
            };
            Pins.Add(pin1);
        }

        public void StopRoute()
        {
            IsVisibleSearchLayout = true;
            IsVisibleStopRouteButton = false;

            MapElements.Clear();
            Pins.Clear();

            _hasRouteRunning = false;
        }

        public async Task GetPlacesByName(string placeText)
        {
            var places = await _mapsApiService.GetPlaces(placeText);
            var placeResult = places.AutoCompletePlaces;
            if (placeResult != null && placeResult.Count > 0)
            {
                Places.Clear();

                foreach(var a in placeResult)
                {
                    Places.Add(a);
                }
            }

            IsShowRecentPlaces = (placeResult == null || placeResult.Count == 0);
        }

        public async Task GetPlacesDetail(GooglePlaceAutoCompletePrediction placeA)
        {
            var place = await _mapsApiService.GetPlaceDetails(placeA.PlaceId);
            if (place != null)
            {
                if (_isPickupFocused)
                {
                    PickupText = place.Name;
                    _origin = new Location(place.Latitude, place.Longitude);
                    _isPickupFocused = false;
                    FocusOriginCommand.Execute(null);
                }
                else
                {
                    _destination = new Location(place.Latitude, place.Longitude);

                    RecentPlaces.Add(placeA);

                    if (_origin.Latitude == _destination.Latitude && _origin.Longitude == _destination.Longitude)
                    {
                        await _alertService.DisplayAlert("Error", "Origin route should be different than destination route", "Ok");
                    }
                    else
                    {
                        LoadRouteCommand.Execute(null);

                        await Shell.Current.GoToAsync("..");

                        CleanFields();
                    }

                }
            }
        }

        private void CleanFields()
        {
            PickupText = OriginText = string.Empty;
            IsShowRecentPlaces = true;
            PlaceSelected = null;
        }
    }
}
