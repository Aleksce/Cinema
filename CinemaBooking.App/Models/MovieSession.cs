namespace CinemaBooking.App.Models;

public class MovieSession
{
    public int Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string HallName { get; set; } = "Зал 1";
    public decimal Price { get; set; } = 450;

    public List<Seat> Seats { get; set; } = [];

    public string DisplayTitle => $"{MovieTitle} • {StartsAt:g} • {HallName} • {Price:0} ₽";
}
