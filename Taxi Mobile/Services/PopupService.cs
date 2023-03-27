using Mopups.Interfaces;
using Taxi_mobile.Interfaces;
using Taxi_mobile.ViewModels.Popups;
using Taxi_mobile.Views.Popups;

namespace Taxi_mobile.Services
{
    public class PopupService : IPopupService
    {
        #region private_fields

        private readonly IPopupNavigation _popupNavigation;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        public PopupService(IPopupNavigation popupNavigation, IServiceProvider serviceProvider) 
        {
            _popupNavigation = popupNavigation;
            _serviceProvider = serviceProvider;
        }

        #region public

        public async Task<string> ShowInfoPopup(string title, string message, bool isPositive = false)
        {
            var popupPage = _serviceProvider.GetRequiredService<InfoPopup>();
            var viewModel = popupPage.BindingContext as InfoPopupViewModel;

            var taskCompletionSource = new TaskCompletionSource<string>();

            viewModel.InitBindings(title, message, isPositive, taskCompletionSource);

            await _popupNavigation.PushAsync(popupPage);

            var result = await taskCompletionSource.Task;

            return result;
        }

        #endregion
    }
}
