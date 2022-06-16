using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace CleanArchitecture.WebUI.Services.Api;

public interface IApiService
{
    Task<List<WeatherForecast>> GetWeatherForecasts();
}
