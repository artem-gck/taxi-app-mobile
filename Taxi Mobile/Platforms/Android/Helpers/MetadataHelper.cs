using Android.Content.PM;

namespace Taxi_mobile.Platforms.Android.Helpers
{
    public static class MetadataHelper
    {
        public static string GetMapsApiKey()
        {
            var context = MauiApplication.Current?.ApplicationContext;
            var packageName = context.PackageName;
            var packageManager = context.PackageManager;
            var metaData = packageManager.GetApplicationInfo(packageName, PackageInfoFlags.MetaData).MetaData;
            var apiKey = metaData.GetString("com.google.android.geo.API_KEY");

            return apiKey;
        }
    }
}
