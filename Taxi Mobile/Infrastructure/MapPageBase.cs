using Taxi_mobile.Interfaces.Platforms;

namespace Taxi_mobile.Infrastructure
{
    public abstract class MapPageBase : BindableBase
    {
        #region private_fields

        protected readonly IPlatformService _platformService;

        private string _title;
        private bool _isBusy = false;

        #endregion

        #region public_fields

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        #endregion

        public MapPageBase(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        #region public

        public bool CanMapPageStart()
        {
            return _platformService.IsGpsOn();
        }

        #endregion
    }
}
