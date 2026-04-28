using Microsoft.Extensions.Configuration;

namespace CinemaBooking.App.Config;

public static class SettingsLoader
{
    public static AppSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables(prefix: "CINEMA_")
            .Build();

        var settings = new AppSettings();
        config.Bind(settings);
        return settings;
    }
}
