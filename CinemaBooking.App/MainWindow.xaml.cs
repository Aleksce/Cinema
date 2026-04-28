using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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
        Loaded += (_, _) =>
        {
            ((Storyboard)FindResource("FadeInStoryboard")).Begin(RootLayout);
            ((Storyboard)FindResource("FadeInStoryboard")).Begin(LoginOverlayCard);
        };
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        _vm.Login(OverlayLoginPasswordBox.Password);
        UpdateLoginOverlay();
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        _vm.Register(OverlayRegisterPasswordBox.Password);
    }

    private void OverlayLogin_Click(object sender, RoutedEventArgs e)
    {
        Login_Click(sender, e);
    }

    private void OverlayRegister_Click(object sender, RoutedEventArgs e)
    {
        Register_Click(sender, e);
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

    private void OpenMovie_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: MovieSession session })
        {
            _vm.OpenMovieModal(session);
            MovieOverlay.Visibility = Visibility.Visible;
            ((Storyboard)FindResource("FadeInStoryboard")).Begin(MovieOverlayCard);
        }
    }

    private void CloseMovie_Click(object sender, RoutedEventArgs e)
    {
        _vm.CloseMovieModal();
        MovieOverlay.Visibility = Visibility.Collapsed;
    }

    private void UpdateLoginOverlay()
    {
        if (_vm.IsAuthenticated)
        {
            LoginOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
