using AutoFixture;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.WebUI.Features.TodoItems;
using CleanArchitecture.WebUI.Services.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace WebUI.UnitTests;

public class WhenTodo
{
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<TodoController>> _mockLogger;
    private readonly Mock<IApiService> _mockApiService;
    private readonly TodosVm _todosVm;
    private readonly TodoController _sut;

    public WhenTodo()
    {

        _mockLogger = new Mock<ILogger<TodoController>>();
        _mockApiService = new Mock<IApiService>();

        _fixture = new Fixture();
        _todosVm = _fixture.Create<TodosVm>();
        _mockApiService.Setup(x => x.GetToDoLists()).Returns(Task.FromResult(_todosVm));

        _sut = new TodoController(_mockApiService.Object, _mockLogger.Object);

    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8601 // Possible null reference assignment.

    [Fact]
    public async Task WhenTheIndexMethodIsCalled_ToDoListsAreReturned()
    {
        
        //Act
        var viewResult = await _sut.Index() as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodosVm;
        model.Should().NotBeNull();
        model.Lists.Should().NotBeNull();
        model.Lists.Should().BeEquivalentTo(_todosVm.Lists);
        model.PriorityLevels.Should().BeEquivalentTo(_todosVm.PriorityLevels);
    }

    [Fact]
    public async Task WhenCreateToDoItem_TodoItemViewModelIsReturned()
    {
        //Arrange
        var todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = true,
            ListId = todoList.Id,
        };

        //Act
        var viewResult = await _sut.Create() as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodoItemViewModel;
        model.Should().BeEquivalentTo(viewModel);

    }

    [Fact]
    public async Task WhenEditToDoItem_TodoItemViewModelIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = false,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };

        //Act
        var viewResult = await _sut.Edit(todoItem.Id) as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodoItemViewModel;
        model.Should().BeEquivalentTo(viewModel);

    }

    [Fact]
    public async Task WhenCreateOrUpdateWithInvalidToDoItem_OriginalViewModelIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = false,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = string.Empty,
            Done = todoItem.Done
        };

        //Act
        var viewResult = await _sut.CreateOrUpdate(viewModel) as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodoItemViewModel;
        model.Should().BeEquivalentTo(viewModel);

    }

    [Fact]
    public async Task WhenCreateOrUpdateAndCreateTodoItemFails_OriginalViewModelIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = true,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };
        _mockApiService.Setup(x => x.CreateTodoItem(It.IsAny<int>(),It.IsAny<string>())).Returns(Task.FromResult(false));

        //Act
        var viewResult = await _sut.CreateOrUpdate(viewModel) as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodoItemViewModel;
        model.Should().BeEquivalentTo(viewModel);

    }

    [Fact]
    public async Task WhenCreateOrUpdateAndUpdateTodoItemFails_OriginalViewModelIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = false,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };
        _mockApiService.Setup(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(false));

        //Act
        var viewResult = await _sut.CreateOrUpdate(viewModel) as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodoItemViewModel;
        model.Should().BeEquivalentTo(viewModel);

    }

    [Fact]
    public async Task WhenCreateOrUpdateAndCreateTodoItemSucceeds_NullIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = true,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };
        _mockApiService.Setup(x => x.CreateTodoItem(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));

        //Act
        var viewResult = await _sut.CreateOrUpdate(viewModel) as ViewResult;

        //Assert
        viewResult.Should().BeNull();
        
    }

    [Fact]
    public async Task WhenCreateOrUpdateAndUpdateTodoItemSucceeds_NullIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        TodoItemViewModel viewModel = new()
        {
            IsCreating = false,
            Id = todoItem.Id,
            ListId = todoList.Id,
            Title = todoItem.Title,
            Done = todoItem.Done
        };
        _mockApiService.Setup(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(true));

        //Act
        var viewResult = await _sut.CreateOrUpdate(viewModel) as ViewResult;

        //Assert
        viewResult.Should().BeNull();

    }

    [Fact]
    public async Task WhenDeleteTodoItemSucceeds_NullIsReturned()
    {
        //Arrange
        TodoListDto? todoList = _todosVm.Lists.FirstOrDefault();
        TodoItemDto? todoItem = todoList.Items.FirstOrDefault();
        
        int deleteTodoItem = 0;
        _mockApiService.Setup(x => x.DeleteTodoItem(It.IsAny<int>()))
            .Callback( () => deleteTodoItem++ )
            .Returns(Task.FromResult(true));

        //Act
        var viewResult = await _sut.Delete(todoItem.Id) as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as TodosVm;
        model.Should().NotBeNull();
        deleteTodoItem.Should().Be(1);

    }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8601 // Possible null reference assignment.
}