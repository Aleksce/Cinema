using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using CinemaBooking.App.Config;
using CinemaBooking.App.Models;

namespace CinemaBooking.App.Integrations;

public class TmdbClient
{
    private readonly TmdbSettings _settings;

    public TmdbClient(TmdbSettings settings)
    {
        _settings = settings;
    }

    public async Task<List<MovieSession>> GetNowPlayingSessionsAsync()
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiKey) && string.IsNullOrWhiteSpace(_settings.ReadAccessToken))
        {
            return [];
        }

        using var client = new HttpClient();
        if (!string.IsNullOrWhiteSpace(_settings.ReadAccessToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ReadAccessToken);
        }

        var url = $"{_settings.BaseUrl}/movie/now_playing?api_key={_settings.ApiKey}&language=ru-RU&page=1";
        try
        {
            var response = await client.GetStringAsync(url);
            var payload = JsonSerializer.Deserialize<TmdbResponse>(response) ?? new TmdbResponse();

            var start = DateTime.Today.AddHours(11);
            var hallCycle = new[] { "IMAX", "Premium", "Стандарт" };
            var sessions = payload.Results.Take(12).Select((movie, i) => new MovieSession
            {
                MovieTitle = movie.Title,
                Overview = movie.Overview,
                PosterUrl = string.IsNullOrWhiteSpace(movie.PosterPath)
                    ? string.Empty
                    : $"{_settings.ImageBaseUrl}{movie.PosterPath}",
                StartsAt = start.AddMinutes(i * 50),
                HallName = hallCycle[i % hallCycle.Length],
                Price = 390 + (i % 4) * 120
            }).ToList();

            return sessions;
        }
        catch
        {
            return [];
        }
    }
}
