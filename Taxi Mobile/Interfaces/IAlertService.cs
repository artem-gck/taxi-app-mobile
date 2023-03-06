namespace Taxi_mobile.Interfaces
{
    public interface IAlertService
    {
        public Task DisplayAlert(string title, string message, string cancel);
    }
}
