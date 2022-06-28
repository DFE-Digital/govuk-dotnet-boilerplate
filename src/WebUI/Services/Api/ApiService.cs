using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using System.Text.Json;
using SFA.DAS.HashingService;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using System.Text;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

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

    public async Task<TodosVm> GetToDoLists()
    {
        using var response = await _client.GetAsync("/api/TodoLists", HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

#pragma warning disable CS8603 // Possible null reference return.
        return await JsonSerializer.DeserializeAsync<TodosVm>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
#pragma warning restore CS8603 // Possible null reference return.

    }

    public async Task<bool> CreateTodoItem(int listId, string title)
    {
        CreateTodoItemCommand command = new()
        {
            ListId = listId,
            Title = title
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/TodoItems"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request); 

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<bool> UpdateTodoItem(int id, string title, bool done)
    {
        UpdateTodoItemCommand command = new()
        {
            Id = id,
            Title = title,
            Done = done
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/TodoItems/{id}"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return true;

    }

    public async Task<bool> DeleteTodoItem(int id)
    {
        using var response = await _client.DeleteAsync(new Uri(_client.BaseAddress + $"api/TodoItems/{id}"));
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return true;
        
        return false;
    }
}
