namespace Taxi_mobile.Infrastructure
{
    public abstract class PopupBase<T> : BindableBase
    {
        #region private_fields

        private string _title;
        private string _message;

        #endregion

        #region public_fields

        public TaskCompletionSource<T> TaskCompletionSource;
        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        #endregion

        public virtual void InitBindings(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
