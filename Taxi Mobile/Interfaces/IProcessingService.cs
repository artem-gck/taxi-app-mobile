using Taxi_mobile.Helpers.Enams;

namespace Taxi_mobile.Interfaces
{
    public interface IProcessingService
    {
        public event Func<ProcessingState, Task> OnCurrentStateChanged;
        public ProcessingState CurrentState { get; }

        public void SetNotActiveState();
        public void SetSelectingDriverState();
    }
}
