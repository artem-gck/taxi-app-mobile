using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Taxi_mobile.Views;
using Taxi_mobile.Views.Controls;
using Taxi_mobile.Views.Handlers;
using Taxi_mobile.Extensions;
using Plugin.LocalNotification;
using Taxi_mobile.ViewModels;

namespace Taxi_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureMopups()
            .ConfigureServices()
            .ConfigurePages()
            .UseLocalNotification()
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

        ConfigureNavigation();

        return builder.Build();
	}

    private static void ConfigureNavigation()
    {
        Routing.RegisterRoute("MapPage", typeof(MapPage));
        Routing.RegisterRoute("SearchPlacePage", typeof(SearchPlacePage));
    }
}
