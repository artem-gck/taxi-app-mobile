using Taxi_mobile.ViewModels;

namespace Taxi_mobile.Views;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel mapViewModel)
	{
		InitializeComponent();
		BindingContext = mapViewModel;
    }
}