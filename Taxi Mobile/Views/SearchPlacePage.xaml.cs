using System.Windows.Input;
using Taxi_mobile.ViewModels;

namespace Taxi_mobile.Views;

public partial class SearchPlacePage : ContentPage
{
    #region bindable_properties

    public static readonly BindableProperty FocusOriginCommandProperty =
           BindableProperty.Create(nameof(FocusOriginCommand), typeof(ICommand), typeof(SearchPlacePage), null, BindingMode.TwoWay);

    public ICommand FocusOriginCommand
    {
        get { return (ICommand)GetValue(FocusOriginCommandProperty); }
        set { SetValue(FocusOriginCommandProperty, value); }
    }

    #endregion

    public SearchPlacePage(SearchPlaceViewModel searchPlaceViewModel)
    {
        InitializeComponent();
        BindingContext = searchPlaceViewModel;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext != null)
        {
            FocusOriginCommand = new Command(OnOriginFocus);
        }
    }

    void OnOriginFocus()
    {
        destinationEntry.Focus();
    }
}