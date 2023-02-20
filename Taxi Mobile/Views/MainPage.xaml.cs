using Taxi_mobile.ViewModels;

namespace Taxi_mobile;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel();
    }
}

