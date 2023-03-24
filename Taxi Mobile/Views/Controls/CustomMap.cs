using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Windows.Input;
using Taxi_mobile.Models.Map;
using Taxi_mobile.Views;

namespace Taxi_mobile.Views.Controls
{
    public class CustomMap : Microsoft.Maui.Controls.Maps.Map
    {
        public static readonly BindableProperty MoveToRegionCommandProperty =
            BindableProperty.Create(nameof(MoveToRegionCommand), typeof(ICommand), typeof(AboutUsPage), null, BindingMode.TwoWay);

        public ICommand MoveToRegionCommand
        {
            get { return (ICommand)GetValue(MoveToRegionCommandProperty); }
            set { SetValue(MoveToRegionCommandProperty, value); }
        }

        public static readonly BindableProperty MapElementsProperty =
            BindableProperty.Create(nameof(MapElements), typeof(IList<MapElement>), typeof(AboutUsPage), null, BindingMode.TwoWay);

        public IList<MapElement> Elements
        {
            get { return (IList<MapElement>)GetValue(MapElementsProperty); }
            set { SetValue(MapElementsProperty, value); }
        }

        public static readonly BindableProperty PinsProperty =
            BindableProperty.Create(nameof(MapElements), typeof(IList<Pin>), typeof(AboutUsPage), null, BindingMode.TwoWay);

        public IList<Pin> Points
        {
            get { return (IList<Pin>)GetValue(PinsProperty); }
            set { SetValue(PinsProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                Elements = MapElements;
                Points = Pins;
                MoveToRegionCommand = new Command<MapSpan>(MoveToRegion);
            }
        }
    }
}
