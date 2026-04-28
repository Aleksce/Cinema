namespace CinemaBooking.App.Config;

public class AppSettings
{
    public TmdbSettings Tmdb { get; set; } = new();
}

public class TmdbSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ReadAccessToken { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.themoviedb.org/3";
    public string ImageBaseUrl { get; set; } = "https://image.tmdb.org/t/p/w500";
}
