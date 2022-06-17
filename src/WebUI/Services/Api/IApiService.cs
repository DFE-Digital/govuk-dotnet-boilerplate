using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace CleanArchitecture.WebUI.Services.Api;

public interface IApiService
{
    Task<List<WeatherForecast>> GetWeatherForecasts();
    Task<TodosVm> GetToDoLists();
    Task<bool> CreateTodoItem(int listId, string title);
    Task<bool> UpdateTodoItem(int id, string title, bool done);
    Task<bool> DeleteTodoItem(int id);
}
