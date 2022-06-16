using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using System.Text.Json;
using SFA.DAS.HashingService;

namespace CleanArchitecture.WebUI.Services.Api;

public class ApiService : IApiService
{
    private readonly HttpClient _client;
    private readonly IHashingService _hashingService;

    public ApiService(HttpClient client, IHashingService hashingService)
    {
        _client = client;
        _hashingService = hashingService;
    }

    public async Task<List<WeatherForecast>> GetWeatherForecasts()
    {
        List<WeatherForecast> weatherForecasts = new();

        using var response = await _client.GetAsync("/api/WeatherForecast", HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var data = await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (data == null)
        {
            weatherForecasts.Add(new WeatherForecast
            {
                Summary = "Failed to get weather forcast"
            });
        }
        else
        {
            weatherForecasts = new List<WeatherForecast>(data);
        }

        return weatherForecasts;
    }
}
