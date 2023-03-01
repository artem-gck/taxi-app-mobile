using Microsoft.Extensions.Logging;
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

        builder.Services.AddSingleton<MapPageViewModel>();
        builder.Services.AddSingleton<MapPage>();

        Routing.RegisterRoute("MapPage", typeof(MapPage));

        return builder.Build();
	}
}
