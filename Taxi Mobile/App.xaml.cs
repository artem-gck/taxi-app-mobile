using Plugin.LocalNotification;
using Taxi_mobile.Helpers;

namespace Taxi_mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += (s, e) =>
        {
            if (!Preferences.ContainsKey(PrefKeys.InstallationIdKey))
            {
                var id = Guid.NewGuid().ToString();
                Preferences.Set(PrefKeys.InstallationIdKey, id);
            }
        };

        return window;
    }
}
