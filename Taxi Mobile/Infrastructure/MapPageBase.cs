using Taxi_mobile.Interfaces.Platforms;

namespace Taxi_mobile.Infrastructure
{
    public class MapPageBase : BindableBase, IQueryAttributable
    {
        protected readonly IPlatformService _platformService;

        private string _title;
        private bool _isBusy = false;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        public MapPageBase(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query) { }

        public bool CanMapPageStart()
        {
            return _platformService.IsGpsOn();
        }
    }
}
