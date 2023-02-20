using System.Windows.Input;
using Taxi_mobile.Infrastructure;

namespace Taxi_mobile.ViewModels
{
    public class MainPageViewModel : MapPageBase
    {
        private int _count = 0;
        private string _text = "Not pressed";

        public string Text { get => _text; set => SetProperty(ref _text, value); }

        public MainPageViewModel()
        {
            Title = "Order car";
        }

        public ICommand Command => new Command(async () =>
        {
            _count++;

            if (_count == 1)
                Text = $"Clicked {_count} time";
            else
                Text = $"Clicked {_count} times";
        });
    }
}
