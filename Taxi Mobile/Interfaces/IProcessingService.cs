using Taxi_mobile.Helpers.Enams;
using Taxi_mobile.Models.Api;

namespace Taxi_mobile.Interfaces
{
    public interface IProcessingService
    {
        public event Action<ProcessingState> OnCurrentStateChanged;
        public ProcessingState CurrentState { get; }

        public Task Initialize();
        public void SetNotActiveState();
        public Task SetNotActiveState(FinishCarRequest finishCarRequest);
        public void SetSelectingDriverState();
        public void SetStartOrderState();
        public Task SetWaitingState(AddOrderRequest addOrderRequest);
        public Task SetEndOfWaiting();
        public Task SetProcessingState();
        public Task SetEndRoadState();
    }
}
