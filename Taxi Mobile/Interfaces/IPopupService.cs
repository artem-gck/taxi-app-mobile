namespace Taxi_mobile.Interfaces
{
    public interface IPopupService
    {
        public Task<string> ShowInfoPopup(string title, string message, bool isPositive = false);
    }
}
