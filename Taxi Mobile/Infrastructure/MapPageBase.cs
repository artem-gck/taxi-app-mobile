namespace Taxi_mobile.Infrastructure
{
    public class MapPageBase : BindableBase
    {
        private string _title;
        private bool _isBusy;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
    }
}
