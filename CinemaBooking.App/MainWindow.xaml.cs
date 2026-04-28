using System.Windows;
using System.Windows.Media;
using CinemaBooking.App.ViewModels;

namespace CinemaBooking.App;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        _vm.Login(LoginPasswordBox.Password);
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        _vm.Register(RegisterPasswordBox.Password);
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        _vm.RefreshSessionsAndSeats();
    }

    private void BookSeat_Click(object sender, RoutedEventArgs e)
    {
        _vm.BookSelectedSeat();
    }

    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        _vm.IsDarkTheme = !_vm.IsDarkTheme;
        Background = _vm.IsDarkTheme
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0A0F1F"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6EEF8"));
    }
}
