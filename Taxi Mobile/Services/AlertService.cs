using Taxi_mobile.Interfaces;

namespace Taxi_mobile.Services
{
    public class AlertService : IAlertService
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
