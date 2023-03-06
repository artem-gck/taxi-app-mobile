using System.Windows.Input;
using Taxi_mobile.Infrastructure;

namespace Taxi_mobile.ViewModels
{
    public class SearchPlaceViewModel : BindableBase
    {
        public ICommand FocusOriginCommand { get; set; }
    }
}
