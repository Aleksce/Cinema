using System.Windows;
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
}
