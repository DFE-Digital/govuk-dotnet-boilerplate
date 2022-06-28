

namespace CleanArchitecture.WebUI.Features.WeatherForecast;

public class WeatherForecastViewModel
{
    public List<CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts.WeatherForecast> WeatherForecasts { get; set; } = default!;
}
