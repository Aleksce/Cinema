namespace CinemaBooking.App.Models;

public class MovieSession
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string HallName { get; set; } = "Зал 1";

    public List<Seat> Seats { get; set; } = [];

    public string DisplayTitle => $"{MovieTitle} | {StartsAt:g} | {HallName}";
}
