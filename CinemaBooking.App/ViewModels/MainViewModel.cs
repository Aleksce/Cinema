using System.Collections.ObjectModel;
using CinemaBooking.App.Models;
using CinemaBooking.App.Services;

namespace CinemaBooking.App.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly AuthService _authService = new();
    private readonly BookingService _bookingService = new();

    private User? _currentUser;
    private MovieSession? _selectedSession;
    private Seat? _selectedSeat;
    private string _loginEmail = string.Empty;
    private string _registerName = string.Empty;
    private string _registerEmail = string.Empty;
    private string _authMessage = string.Empty;
    private string _registerMessage = string.Empty;
    private string _bookingMessage = string.Empty;
    private string _syncMessage = string.Empty;
    private bool _isDarkTheme = true;
    private bool _isMovieModalOpen;

    public ObservableCollection<MovieSession> Sessions { get; } = [];
    public ObservableCollection<Seat> Seats { get; } = [];
    public ObservableCollection<Booking> MyBookings { get; } = [];

    public string LoginEmail
    {
        get => _loginEmail;
        set => SetProperty(ref _loginEmail, value);
    }

    public string RegisterName
    {
        get => _registerName;
        set => SetProperty(ref _registerName, value);
    }

    public string RegisterEmail
    {
        get => _registerEmail;
        set => SetProperty(ref _registerEmail, value);
    }

    public string AuthMessage
    {
        get => _authMessage;
        set => SetProperty(ref _authMessage, value);
    }

    public string RegisterMessage
    {
        get => _registerMessage;
        set => SetProperty(ref _registerMessage, value);
    }

    public string BookingMessage
    {
        get => _bookingMessage;
        set => SetProperty(ref _bookingMessage, value);
    }

    public string SyncMessage
    {
        get => _syncMessage;
        set => SetProperty(ref _syncMessage, value);
    }

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set => SetProperty(ref _isDarkTheme, value);
    }

    public bool IsMovieModalOpen
    {
        get => _isMovieModalOpen;
        set => SetProperty(ref _isMovieModalOpen, value);
    }

    public bool IsAuthenticated => _currentUser is not null;

    public string CurrentUserDisplay => _currentUser is null ? "Гость" : _currentUser.Name;
    public string SelectedMovieTitle => SelectedSession?.MovieTitle ?? "Выберите фильм";
    public string SelectedMovieOverview => SelectedSession?.Overview ?? "Описание появится после выбора фильма.";
    public string SelectedMoviePrice => SelectedSession is null ? "—" : $"{SelectedSession.Price:0} ₽";
    public string SelectedMoviePoster => SelectedSession?.PosterUrl ?? string.Empty;

    public MovieSession? SelectedSession
    {
        get => _selectedSession;
        set
        {
            if (SetProperty(ref _selectedSession, value))
            {
                LoadSeats();
                OnPropertyChanged(nameof(SelectedMovieTitle));
                OnPropertyChanged(nameof(SelectedMovieOverview));
                OnPropertyChanged(nameof(SelectedMoviePrice));
                OnPropertyChanged(nameof(SelectedMoviePoster));
            }
        }
    }

    public Seat? SelectedSeat
    {
        get => _selectedSeat;
        set => SetProperty(ref _selectedSeat, value);
    }

    public MainViewModel()
    {
        RefreshSessionsAndSeats();
    }

    public void Login(string password)
    {
        var user = _authService.Login(LoginEmail.Trim(), password);
        if (user is null)
        {
            AuthMessage = "Неверный логин или пароль.";
            return;
        }

        _currentUser = user;
        AuthMessage = "Вход выполнен.";
        LoadMyBookings();
        OnPropertyChanged(nameof(CurrentUserDisplay));
        OnPropertyChanged(nameof(IsAuthenticated));
    }

    public void Register(string password)
    {
        var result = _authService.Register(RegisterName.Trim(), RegisterEmail.Trim(), password);
        RegisterMessage = result.Message;
    }

    public void RefreshSessionsAndSeats()
    {
        Sessions.Clear();
        foreach (var session in _bookingService.GetSessions())
        {
            Sessions.Add(session);
        }

        SelectedSession = Sessions.FirstOrDefault();
        LoadSeats();
    }

    public async Task SyncTmdbAsync()
    {
        SyncMessage = "Синхронизация с TMDB...";
        var result = await _bookingService.SyncSessionsFromTmdbAsync();
        SyncMessage = result.Message;
        RefreshSessionsAndSeats();
    }

    public void OpenMovieModal(MovieSession session)
    {
        SelectedSession = session;
        IsMovieModalOpen = true;
    }

    public void CloseMovieModal()
    {
        IsMovieModalOpen = false;
    }

    public void BookSelectedSeat()
    {
        if (_currentUser is null)
        {
            BookingMessage = "Сначала выполните вход.";
            return;
        }

        if (SelectedSeat is null)
        {
            BookingMessage = "Выберите место.";
            return;
        }

        var result = _bookingService.BookSeat(_currentUser.Id, SelectedSeat.Id);
        BookingMessage = result.Message;
        LoadSeats();
        LoadMyBookings();
    }

    private void LoadSeats()
    {
        Seats.Clear();
        if (SelectedSession is null)
        {
            return;
        }

        foreach (var seat in _bookingService.GetSeatsForSession(SelectedSession.Id))
        {
            Seats.Add(seat);
        }
    }

    private void LoadMyBookings()
    {
        MyBookings.Clear();
        if (_currentUser is null)
        {
            return;
        }

        foreach (var booking in _bookingService.GetBookingsForUser(_currentUser.Id))
        {
            MyBookings.Add(booking);
        }
    }
}
