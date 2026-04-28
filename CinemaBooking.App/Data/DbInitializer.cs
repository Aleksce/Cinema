using CinemaBooking.App.Config;
using CinemaBooking.App.Integrations;
using CinemaBooking.App.Models;

namespace CinemaBooking.App.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync()
    {
        using var db = new CinemaDbContext();
        db.Database.EnsureCreated();

        if (db.Sessions.Any())
        {
            return;
        }

        var settings = SettingsLoader.Load();
        var tmdbClient = new TmdbClient(settings.Tmdb);
        var sessions = await tmdbClient.GetNowPlayingSessionsAsync();

        if (sessions.Count == 0)
        {
            sessions =
            [
                new() { MovieTitle = "Dune: Part Two", Overview = "Эпическая фантастика.", StartsAt = DateTime.Today.AddHours(18), HallName = "Зал A", Price = 450 },
                new() { MovieTitle = "Interstellar", Overview = "Космическая драма.", StartsAt = DateTime.Today.AddHours(21), HallName = "Зал B", Price = 500 }
            ];
        }

        foreach (var session in sessions)
        {
            for (var row = 1; row <= 6; row++)
            {
                for (var seatNo = 1; seatNo <= 10; seatNo++)
                {
                    session.Seats.Add(new Seat
                    {
                        Row = row,
                        Number = seatNo,
                        IsBooked = false
                    });
                }
            }
        }

        db.Sessions.AddRange(sessions);
        db.SaveChanges();
    }
}
