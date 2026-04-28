using System.Windows;
using CinemaBooking.App.Data;

namespace CinemaBooking.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DbInitializer.Initialize();
    }
}
