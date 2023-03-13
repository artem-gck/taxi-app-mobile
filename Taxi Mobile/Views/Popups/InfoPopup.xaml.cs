using Taxi_mobile.ViewModels.Popups;

namespace Taxi_mobile.Views.Popups;

public partial class InfoPopup
{
	public InfoPopup(InfoPopupViewModel infoPopupViewModel)
	{
		InitializeComponent();
		BindingContext = infoPopupViewModel;
	}
}