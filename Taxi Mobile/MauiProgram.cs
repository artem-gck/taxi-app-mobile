using Microsoft.Extensions.Logging;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Services;
using Taxi_mobile.Services.Platforms;
using Taxi_mobile.ViewModels;
using Taxi_mobile.Views;

namespace Taxi_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .UseMauiMaps();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
		builder.Services.AddSingleton<IMapsApiService, MapsApiService>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
		builder.Services.AddSingleton<IGeolocationService, GeolocationService>();

        builder.Services.AddSingleton<MapPageViewModel>();
        builder.Services.AddSingleton<AboutUsViewModel>();
        builder.Services.AddSingleton<MapPage>();
        builder.Services.AddSingleton<AboutUsPage>();
        builder.Services.AddSingleton<SearchPlacePage>();

        Routing.RegisterRoute("MapPage", typeof(MapPage));
        Routing.RegisterRoute("SearchPlacePage", typeof(SearchPlacePage));

        return builder.Build();
	}
}
