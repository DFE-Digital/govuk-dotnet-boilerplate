using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.WebUI.Services.Api;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Features.TodoItems;
public class TodoController : Controller
{
    private readonly ILogger<TodoController> _logger;
    private readonly IApiService _apiService;

    public TodoController(IApiService apiService, ILogger<TodoController> logger)
    {
        _logger = logger;
        _apiService = apiService;
    }
    
    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetToDoLists();
        return View(result);
    }

    public async Task<IActionResult> Create()
    {
        var result = await _apiService.GetToDoLists();
        TodoListDto? todoList = result.Lists.FirstOrDefault();
        ArgumentNullException.ThrowIfNull(todoList);

        return View("ToDoItem", new TodoItemViewModel()
        {
            IsCreating = true,
            ListId = todoList.Id,
        });

    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _apiService.GetToDoLists();
        TodoListDto? todoList = result.Lists.FirstOrDefault();
        ArgumentNullException.ThrowIfNull(todoList);
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault(x => x.Id == id);
        ArgumentNullException.ThrowIfNull(todoItem);
        return View("ToDoItem", new TodoItemViewModel() 
        { 
            IsCreating = false, 
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        });

    }


    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate(TodoItemViewModel viewModel)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(viewModel.Title))
        {
            return View("ToDoItem", viewModel);
        }

        if (viewModel.IsCreating)
        {
            if (!await _apiService.CreateTodoItem(viewModel.ListId, viewModel.Title))
            {
                return View("ToDoItem", viewModel);
            }
        }
        else
        {
            if (!await _apiService.UpdateTodoItem(viewModel.Id, viewModel.Title, viewModel.Done))
            {
                return View("ToDoItem", viewModel);
            }
        }
        
        return Redirect("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        _ = await _apiService.DeleteTodoItem(id);

        var result = await _apiService.GetToDoLists();
        return View("Index",result);

    }
}
