namespace CinemaBooking.App.Models;

public class Seat
{
    public int Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public bool IsBooked { get; set; }

    public int MovieSessionId { get; set; }
    public MovieSession? MovieSession { get; set; }

    public Booking? Booking { get; set; }
}
