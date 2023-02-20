namespace Taxi_mobile.Infrastructure
{
    public class MapPageBase : BindableBase
    {
        private string _title;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
    }
}
