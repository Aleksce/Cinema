namespace CinemaBooking.App.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User? User { get; set; }

    public int SeatId { get; set; }
    public Seat? Seat { get; set; }

    public MovieSession? Session => Seat?.MovieSession;
}
