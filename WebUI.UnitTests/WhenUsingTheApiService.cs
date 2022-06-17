using System.Text;
using System.Text.Json;
using AutoFixture;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using CleanArchitecture.WebUI.Services.Api;
using FluentAssertions;
using Moq;
using Moq.Protected;
using SFA.DAS.HashingService;

namespace WebUI.UnitTests;
public class WhenUsingTheApiService
{
 
    private readonly Mock<IHashingService> _mockHashingService;
    private readonly Fixture _fixture;

    public WhenUsingTheApiService()
    {
        _fixture = new Fixture();
        _mockHashingService = new Mock<IHashingService>();
    }

    private ApiService GetApiService(string json)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        // Setup Protected method on HttpMessageHandler mock.
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
            {
                HttpResponseMessage response = new()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                return response;
            });

        return new ApiService(new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://www.someurl.com")
        }, _mockHashingService.Object);
    }

    [Fact]
    public async Task ThenGetWeatherForecasts_ReturnsListWeatherForcasts()
    {
        //Arrange
        List<WeatherForecast>  weatherForcasts = _fixture.Create<List<WeatherForecast>>();
        string json = string.Empty;
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, weatherForcasts);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            json = await reader.ReadToEndAsync();
        }

        ApiService sut = GetApiService(json);


        //Act
        var result = await sut.GetWeatherForecasts();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(weatherForcasts);
    }

    [Fact]
    public async Task ThenGetToDoLists_ReturnsTodoVM()
    {
        //Arrange
        TodosVm todoVM = _fixture.Create<TodosVm>();
        string json = string.Empty;
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, todoVM);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            json = await reader.ReadToEndAsync();
        }

        ApiService sut = GetApiService(json);


        //Act
        var result = await sut.GetToDoLists();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(todoVM);
    }

    

    [Fact]
    public async Task ThenCreateTodoItem_ReturnsSuccessful()
    {
        //Arrange
        bool retVal = true;
        string json = string.Empty;
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, retVal);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            json = await reader.ReadToEndAsync();
        }

        ApiService sut = GetApiService(json);


        //Act
        var result = await sut.CreateTodoItem(1,"Title");

        //Assert
        result.Should().BeTrue();
        
    }

    [Fact]
    public async Task ThenUpdateTodoItem_ReturnsSuccessful()
    {
        //Arrange
        bool retVal = true;
        string json = string.Empty;
        using (var stream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(stream, retVal);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            json = await reader.ReadToEndAsync();
        }

        ApiService sut = GetApiService(json);


        //Act
        var result = await sut.UpdateTodoItem(1, "Title", false);

        //Assert
        result.Should().BeTrue();

    }

    [Fact]
    public async Task ThenDeleteTodoItem_ReturnsSuccessful()
    {
        //Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        // Setup Protected method on HttpMessageHandler mock.
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
            {
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.NoContent,
                    
                };

                return response;
            });

        ApiService sut = new ApiService(new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://www.someurl.com")
        }, _mockHashingService.Object);

        //Act
        var result = await sut.DeleteTodoItem(1);

        //Assert
        result.Should().BeTrue();

    }
}
