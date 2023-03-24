using Mopups.Interfaces;
using Mopups.Services;
using Plugin.LocalNotification;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Interfaces.Platforms;
using Taxi_mobile.Services;
using Taxi_mobile.Services.Platforms;
using Taxi_mobile.ViewModels;
using Taxi_mobile.ViewModels.Popups;
using Taxi_mobile.Views;
using Taxi_mobile.Views.Popups;

namespace Taxi_mobile.Extensions
{
    public static class BuilderExtension
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
            builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);
            builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
            builder.Services.AddSingleton<INotificationService>(LocalNotificationCenter.Current);

            builder.Services.AddSingleton<IMapsApiService, MapsApiService>();
            builder.Services.AddSingleton<IPlatformService, PlatformService>();

            builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
            builder.Services.AddSingleton<IPopupService, PopupService>();
            builder.Services.AddSingleton<IWebService, WebService>();
            builder.Services.AddSingleton<IProcessingService, ProcessingService>();
            builder.Services.AddSingleton<ILocalNotificationService, LocalNotificationService>();

            return builder;
        }

        public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<AboutUsViewModel>();
            builder.Services.AddTransient<SearchPlaceViewModel>();
            builder.Services.AddSingleton<InfoPopupViewModel>();

            builder.Services.AddSingleton<AboutUsPage>();
            builder.Services.AddTransient<SearchPlacePage>();
            builder.Services.AddSingleton<InfoPopup>();

            return builder;
        }
    }
}
