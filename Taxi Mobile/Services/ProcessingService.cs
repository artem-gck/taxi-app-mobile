using Taxi_mobile.Helpers;
using Taxi_mobile.Helpers.Enams;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Models.Api;

namespace Taxi_mobile.Services
{
    public class ProcessingService : IProcessingService
    {
        private readonly IWebService _webService;
        private readonly ILocalNotificationService _localNotificationService;

        private ProcessingState _currentState = ProcessingState.NotActive;
        private Guid _orderId;

        public event Action<ProcessingState> OnCurrentStateChanged;
        public ProcessingState CurrentState 
        { 
            get => _currentState; 
            private set 
            {
                _currentState = value;
                OnCurrentStateChanged(value);
            }
        }

        public ProcessingService(IWebService webService, ILocalNotificationService localNotificationService)
        {
            _webService = webService;
            _localNotificationService = localNotificationService;
        }

        public async Task Initialize()
        {
            var response = await _webService.GetUserState(AppConstants.UserId);

            CurrentState = response.State switch
            {
                "Free"          => ProcessingState.NotActive,
                "Waiting car"   => ProcessingState.Waiting,
                "On the trip"   => ProcessingState.Processing,
                _               => throw new NotImplementedException()
            };
        }

        public void SetNotActiveState()
        {
            CurrentState = ProcessingState.NotActive;
        }

        public async Task SetNotActiveState(FinishCarRequest finishCarRequest)
        {
            if (CurrentState == ProcessingState.EndRoad)
            {
                finishCarRequest.OrderId = _orderId;

                await _webService.PutFinishOrder(_orderId, finishCarRequest);

                CurrentState = ProcessingState.NotActive;
            }
        }

        public void SetSelectingDriverState()
        {
            if (CurrentState == ProcessingState.NotActive)
            {
                CurrentState = ProcessingState.SelectingDriver;
            }
        }

        public void SetStartOrderState()
        {
            if (CurrentState == ProcessingState.SelectingDriver)
            {
                CurrentState = ProcessingState.StartOrder;
            }
        }

        public async Task SetWaitingState(AddOrderRequest addOrderRequest)
        {
            if (CurrentState == ProcessingState.StartOrder)
            {
                var result = await _webService.PostAddOrder(addOrderRequest);
                _orderId = result.OrderId;

                CurrentState = ProcessingState.Waiting;
            }
        }

        public async Task SetEndOfWaiting()
        {
            if (CurrentState == ProcessingState.Waiting)
            {
                await _localNotificationService.SendInfoNotification("Taxi is hear", "Your taxi is near you");

                CurrentState = ProcessingState.EndOfWaiting;
            }
        }

        public async Task SetProcessingState()
        {
            if (CurrentState == ProcessingState.EndOfWaiting)
            {
                _localNotificationService.CancelNotifications();

                await _webService.PutProcessOrder(_orderId);

                CurrentState = ProcessingState.Processing;
            }
        }

        public async Task SetEndRoadState()
        {
            if (CurrentState == ProcessingState.Processing)
            { 
                CurrentState = ProcessingState.EndRoad;
            }
        }
    }
}
