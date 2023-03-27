using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Taxi_mobile.Interfaces;

namespace Taxi_mobile.Services
{
    public class LocalNotificationService : ILocalNotificationService
    {
        #region private_fields

        private readonly INotificationService _notificationService;
        private readonly ILogger<LocalNotificationService> _logger;

        private List<int> _pushedNotifications = new List<int>();

        #endregion

        public LocalNotificationService(INotificationService notificationService, ILogger<LocalNotificationService> logger) 
        {
            _notificationService = notificationService;
            _logger = logger;
            
            notificationService.NotificationActionTapped += NotificationService_NotificationActionTapped;
        }

        #region public

        public async Task SendInfoNotification(string title, string subTitle)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(subTitle))
            {
                var notificationId = GenerateNotificationId();

                var request = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = title,
                    Subtitle = subTitle,
                    BadgeNumber = 42,
                    CategoryType = NotificationCategoryType.Status,
                };

                await _notificationService.Show(request);
            }
        }

        public void CancelNotifications()
        {
            _notificationService.CancelAll();
            _pushedNotifications.Clear();
        }

        #endregion

        #region private

        private int GenerateNotificationId()
        {
            var rand = Random.Shared;
            var id = rand.Next(999);

            while (_pushedNotifications.Contains(id))
                id = rand.Next(999);

            _pushedNotifications.Add(id);

            return id;
        }

        private void NotificationService_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {

            }
            else if (e.IsTapped)
            {

            }
        }

        #endregion
    }
}
