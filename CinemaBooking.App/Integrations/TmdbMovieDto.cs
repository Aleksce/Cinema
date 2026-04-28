using System.Text.Json.Serialization;

namespace CinemaBooking.App.Integrations;

public class TmdbResponse
{
    [JsonPropertyName("results")]
    public List<TmdbMovieDto> Results { get; set; } = [];
}

public class TmdbMovieDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("overview")]
    public string Overview { get; set; } = string.Empty;

    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = string.Empty;
}
