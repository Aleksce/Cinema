using CinemaBooking.App.Config;
using CinemaBooking.App.Data;
using CinemaBooking.App.Integrations;
using CinemaBooking.App.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.App.Services;

public class BookingService
{
    public List<MovieSession> GetSessions()
    {
        using var db = new CinemaDbContext();
        return db.Sessions.OrderBy(x => x.StartsAt).ToList();
    }

    public async Task<(bool Success, string Message)> SyncSessionsFromTmdbAsync()
    {
        var settings = SettingsLoader.Load();
        var tmdbClient = new TmdbClient(settings.Tmdb);
        var externalSessions = await tmdbClient.GetNowPlayingSessionsAsync();

        if (externalSessions.Count == 0)
        {
            return (false, "TMDB не вернул фильмы. Проверьте ключ и интернет-соединение.");
        }

        using var db = new CinemaDbContext();

        var oldBookings = db.Bookings.ToList();
        db.Bookings.RemoveRange(oldBookings);

        var oldSeats = db.Seats.ToList();
        db.Seats.RemoveRange(oldSeats);

        var oldSessions = db.Sessions.ToList();
        db.Sessions.RemoveRange(oldSessions);

        foreach (var session in externalSessions)
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

        db.Sessions.AddRange(externalSessions);
        await db.SaveChangesAsync();
        return (true, $"Загружено {externalSessions.Count} фильмов из TMDB.");
    }

    public List<Seat> GetSeatsForSession(int sessionId)
    {
        using var db = new CinemaDbContext();
        return db.Seats
            .Where(x => x.MovieSessionId == sessionId)
            .OrderBy(x => x.Row)
            .ThenBy(x => x.Number)
            .ToList();
    }

    public (bool Success, string Message) BookSeat(int userId, int seatId)
    {
        using var db = new CinemaDbContext();
        var seat = db.Seats.FirstOrDefault(x => x.Id == seatId);
        if (seat is null)
        {
            return (false, "Место не найдено.");
        }

        if (seat.IsBooked)
        {
            return (false, "Это место уже занято.");
        }

        seat.IsBooked = true;
        db.Bookings.Add(new Booking
        {
            UserId = userId,
            SeatId = seat.Id,
            BookedAt = DateTime.UtcNow
        });

        db.SaveChanges();
        return (true, "Место успешно забронировано.");
    }

    public List<Booking> GetBookingsForUser(int userId)
    {
        using var db = new CinemaDbContext();
        return db.Bookings
            .Include(x => x.Seat)
            .ThenInclude(x => x!.MovieSession)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.BookedAt)
            .ToList();
    }
}
