using Taxi_mobile.ViewModels;

namespace Taxi_mobile.Views;

public partial class MapPage : ContentPage
{
	public MapPage(MapPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}