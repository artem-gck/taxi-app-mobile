using System.Collections.ObjectModel;
using System.Windows.Input;
using Taxi_mobile.Helpers;
using Taxi_mobile.Infrastructure;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Models.Db;
using Taxi_mobile.Models.GoogleMaps;
using Taxi_mobile.Resources.Dictionaries;

namespace Taxi_mobile.ViewModels
{
    public class SearchPlaceViewModel : BindableBase
    {
        private readonly IMapsApiService _mapsApiService;
        private readonly IGeolocationService _geolocationService;
        private readonly IPopupService _popupService;
        private readonly IDbService _dbService;

        private string _pickupText;
        private string _originText;
        private bool _isShowRecentPlaces = true;
        private bool _isPickupFocused = true;
        private bool _isChooseLocation = false;
        private GooglePlaceAutoCompletePrediction _placeSelected;
        private Location _origin;
        private Location _destination;
        private List<Location> positions;


        public ICommand FocusOriginCommand { get; set; }
        public ICommand ChooseMyLocationCommand { get; set; }

        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> RecentPlaces { get; set; }
        public bool IsShowRecentPlaces 
        { 
            get => _isShowRecentPlaces; 
            set => SetProperty(ref _isShowRecentPlaces, value); 
        }
        public string PickupText
        {
            get => _pickupText;
            set => SetProperty(ref _pickupText, value, () => {
                if (!string.IsNullOrEmpty(_pickupText) && value != ResourcesViewModel.CurrentLocation)
                {
                    _isPickupFocused = true;
                    GetPlacesByName(_pickupText);
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
                    GetPlacesByName(_originText);
                }
            });
        }
        public GooglePlaceAutoCompletePrediction PlaceSelected
        {
            get => _placeSelected;
            set => SetProperty(ref _placeSelected, value, () =>
            {
                if (_placeSelected != null)
                    GetPlacesDetail(_placeSelected);
            });
        }

        public SearchPlaceViewModel(IMapsApiService mapsApiService, IGeolocationService geolocationService, IPopupService popupService, IDbService dbService) 
        {
            _mapsApiService = mapsApiService;
            _geolocationService = geolocationService;
            _popupService = popupService;
            _dbService = dbService;

            Places = new ();
            RecentPlaces = new ();

            ChooseMyLocationCommand = new Command(async () => await ChooseCurentLocation());
        }

        public override async void ApplyQueryAttributes(IDictionary<string, object> query) 
        {
            IsShowRecentPlaces = true;

            var recentPlaceEntities = await _dbService.GetTopByDateAsync(AppConstants.CountOfRecentPlaces);

            foreach (var place in recentPlaceEntities)
            {
                RecentPlaces.Add(MapEntityToPrediction(place));
            }
        }

        private async Task ChooseCurentLocation()
        {
            _isChooseLocation = false;

            _origin = await _geolocationService.GetCurrentLocationAsync(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));

            PickupText = ResourcesViewModel.CurrentLocation;

            _isPickupFocused = false;
            _isChooseLocation = true;

            GetPlacesDetail(null);

            _isChooseLocation = false;
        }

        private async Task LoadRoute()
        {
            var positionIndex = 1;
            var googleDirection = await _mapsApiService.GetDirections(_origin, _destination);
            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {
                positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));

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
                //await _alertService.DisplayAlert(":(", "No route found", "Ok");
            }

        }

        private async void GetPlacesByName(string placeText)
        {
            var places = await _mapsApiService.GetPlaces(placeText);
            var placeResult = places.AutoCompletePlaces;

            if (placeResult != null && placeResult.Count > 0)
            {
                Places.Clear();

                foreach (var a in placeResult)
                {
                    Places.Add(a);
                }
            }

            IsShowRecentPlaces = (placeResult == null || placeResult.Count == 0);
        }

        private async void GetPlacesDetail(GooglePlaceAutoCompletePrediction placeA)
        {
            if (_isChooseLocation)
            {
                FocusOriginCommand.Execute(null);

                return;
            }

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
                    OriginText = place.Name;

                    _destination = new Location(place.Latitude, place.Longitude);

                    await _dbService.AddOrUpdateAsync(MapPredictionToEntity(placeA));

                    if (_origin.Latitude == _destination.Latitude && _origin.Longitude == _destination.Longitude)
                    {
                        //await _alertService.DisplayAlert("Error", "Origin route should be different than destination route", "Ok");
                    }
                    else
                    {
                        await LoadRoute();

                        var navigationParameter = new Dictionary<string, object>
                        {
                            { "positions", positions },
                            { "origin", _origin },
                            { "destination", _destination }
                        };

                        await Shell.Current.GoToAsync($"..", navigationParameter);

                        CleanFields();
                    }
                }

                PlaceSelected = null;
            }
        }

        private void CleanFields()
        {
            PickupText = string.Empty;
            OriginText = string.Empty;
            IsShowRecentPlaces = true;
            PlaceSelected = null;
        }

        private GooglePlaceAutoCompletePrediction MapEntityToPrediction(PlaceAutoCompletePredictionEntity entity)
            => new GooglePlaceAutoCompletePrediction()
            {
                Description = entity.Description,
                Id = entity.GoogleId,
                PlaceId = entity.PlaceId,
                Reference = entity.Reference,
                StructuredFormatting = new StructuredFormatting()
                {
                    MainText = entity.StructuredFormatting.MainText,
                    SecondaryText = entity.StructuredFormatting.SecondaryText,
                }
            };

        private PlaceAutoCompletePredictionEntity MapPredictionToEntity(GooglePlaceAutoCompletePrediction prediction)
            => new PlaceAutoCompletePredictionEntity()
            {
                Description = prediction.Description,
                GoogleId = prediction.Id,
                PlaceId = prediction.PlaceId,
                Reference = prediction.Reference,
                StructuredFormatting = new StructuredFormattingEntity()
                {
                    MainText = prediction.StructuredFormatting.MainText,
                    SecondaryText = prediction.StructuredFormatting.SecondaryText,
                }
            };
    }
}
