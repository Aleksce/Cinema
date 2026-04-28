using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CinemaBooking.App.Models;
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

    private async void SyncTmdb_Click(object sender, RoutedEventArgs e)
    {
        await _vm.SyncTmdbAsync();
    }

    private void BookSeat_Click(object sender, RoutedEventArgs e)
    {
        _vm.BookSelectedSeat();
    }

    private void Seat_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Seat seat })
        {
            _vm.SelectedSeat = seat;
            _vm.BookingMessage = $"Выбрано место: ряд {seat.Row}, место {seat.Number}";
        }
    }

    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        _vm.IsDarkTheme = !_vm.IsDarkTheme;
        Background = _vm.IsDarkTheme
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0A0F1F"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6EEF8"));
    }
}
