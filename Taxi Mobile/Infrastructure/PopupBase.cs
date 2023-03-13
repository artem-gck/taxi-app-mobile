namespace Taxi_mobile.Infrastructure
{
    public abstract class PopupBase<T> : BindableBase
    {
        private string _title;
        private string _message;

        public TaskCompletionSource<T> TaskCompletionSource;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public virtual void InitBindings(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
