using CinemaBooking.App.Data;
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
