using CleanArchitecture.WebUI.Services.Api;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Features.WeatherForecast;
public class WeatherForecastController : Controller
{
    private readonly IApiService _apiService;

    public WeatherForecastController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var weatherForecasts = new WeatherForecastViewModel()
        {
            WeatherForecasts = await _apiService.GetWeatherForecasts()
        };

        return View(weatherForecasts);
    }
}
