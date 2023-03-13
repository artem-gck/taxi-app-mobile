using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Services;
using Taxi_mobile.Services.Platforms;
using Taxi_mobile.ViewModels;
using Taxi_mobile.ViewModels.Popups;
using Taxi_mobile.Views;
using Taxi_mobile.Views.Popups;
using Taxi_mobile.Views.Controls;
using Taxi_mobile.Views.Handlers;

namespace Taxi_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureMopups()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CustomMap, CustomMapHandler>();
            })
            .UseMauiMaps();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
		builder.Services.AddSingleton<IMapsApiService, MapsApiService>();
		builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
        builder.Services.AddSingleton<IPlatformService, PlatformService>();
        builder.Services.AddSingleton<IPopupService, PopupService>();
        builder.Services.AddSingleton<IWebService, WebService>();
        builder.Services.AddSingleton<IProcessingService, ProcessingService>();

        builder.Services.AddSingleton<AboutUsViewModel>();
        builder.Services.AddSingleton<SearchPlaceViewModel>();
        builder.Services.AddSingleton<InfoPopupViewModel>();

        builder.Services.AddSingleton<AboutUsPage>();
        builder.Services.AddSingleton<SearchPlacePage>();
        builder.Services.AddSingleton<InfoPopup>();

        Routing.RegisterRoute("AboutUsPage", typeof(AboutUsPage));
        Routing.RegisterRoute("SearchPlacePage", typeof(SearchPlacePage));

        return builder.Build();
	}
}
