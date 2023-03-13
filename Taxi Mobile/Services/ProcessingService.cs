using Taxi_mobile.Helpers.Enams;
using Taxi_mobile.Interfaces;

namespace Taxi_mobile.Services
{
    public class ProcessingService : IProcessingService
    {
        private ProcessingState _currentState = ProcessingState.NotActive;

        public event Func<ProcessingState, Task> OnCurrentStateChanged;
        public ProcessingState CurrentState 
        { 
            get => _currentState; 
            private set 
            {
                _currentState = value;
                OnCurrentStateChanged(value);
            }
        }

        public void SetNotActiveState()
        {
            CurrentState = ProcessingState.NotActive;
        }

        public void SetSelectingDriverState()
        {
            if (CurrentState == ProcessingState.NotActive)
            {
                CurrentState = ProcessingState.SelectingDriver;
            }
        }
    }
}
