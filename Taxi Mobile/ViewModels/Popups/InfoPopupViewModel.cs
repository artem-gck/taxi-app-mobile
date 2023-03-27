using Mopups.Interfaces;
using Taxi_mobile.Infrastructure;
using Taxi_mobile.Interfaces.Platforms;

namespace Taxi_mobile.ViewModels.Popups
{
    public class InfoPopupViewModel : PopupBase<string>
    {
        #region private_fields

        private readonly IPlatformService _platformService;
        private readonly IPopupNavigation _popupNavigation;

        private bool _isPositive;

        #endregion

        #region public_fields

        public bool IsPositive { get => _isPositive; set => SetProperty(ref _isPositive, value); }

        #endregion

        public InfoPopupViewModel(IPlatformService platformService, IPopupNavigation popupNavigation) 
        {
            _platformService = platformService;
            _popupNavigation = popupNavigation;

            Initialize();
        }

        #region public

        public void InitBindings(string title, string message, bool isPositive, TaskCompletionSource<string> taskCompletionSource)
        {
            base.InitBindings(title, message);

            IsPositive = isPositive;
            TaskCompletionSource = taskCompletionSource;
        }

        #endregion

        #region private

        private void Initialize()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => CheckGpsCallBack());
        }

        private bool CheckGpsCallBack()
        {
            var isGpsOn = _platformService.IsGpsOn();

            if (isGpsOn)
            {
                _popupNavigation.PopAsync();
                TaskCompletionSource.SetResult("closed");
            }

            return !isGpsOn;
        }

        #endregion
    }
}
