using Taxi_mobile.ViewModels;

namespace Taxi_mobile.Views;

public partial class AboutUsPage : ContentPage
{
	public AboutUsPage(AboutUsViewModel aboutUsViewModel)
	{
		InitializeComponent();
		BindingContext = aboutUsViewModel;
    }
}