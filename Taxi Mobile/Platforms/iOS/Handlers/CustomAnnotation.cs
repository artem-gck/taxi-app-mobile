using MapKit;
using Microsoft.Maui.Maps;
using UIKit;

namespace Taxi_mobile.Views.Handlers
{
    public class CustomAnnotation : MKPointAnnotation
    {
        public Guid Identifier { get; init; }
        public UIImage? Image { get; init; }
        public required IMapPin Pin { get; init; }
    }
}
