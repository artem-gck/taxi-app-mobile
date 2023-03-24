namespace Taxi_mobile.Interfaces
{
    public interface ILocalNotificationService
    {
        public Task SendInfoNotification(string title, string subTitle);
        public void CancelNotifications();
    }
}
