using CinemaBooking.App.Models;

namespace CinemaBooking.App.Data;

public static class DbInitializer
{
    public static void Initialize()
    {
        using var db = new CinemaDbContext();
        db.Database.EnsureCreated();

        if (db.Sessions.Any())
        {
            return;
        }

        var sessions = new List<MovieSession>
        {
            new() { MovieTitle = "Dune: Part Two", StartsAt = DateTime.Today.AddHours(18), HallName = "Зал A" },
            new() { MovieTitle = "Interstellar", StartsAt = DateTime.Today.AddHours(21), HallName = "Зал B" }
        };

        foreach (var session in sessions)
        {
            for (var row = 1; row <= 5; row++)
            {
                for (var seatNo = 1; seatNo <= 8; seatNo++)
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
