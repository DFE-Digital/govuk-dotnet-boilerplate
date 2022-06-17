using AutoFixture;
using CleanArchitecture.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using CleanArchitecture.WebUI.Services.Api;
using CleanArchitecture.WebUI.Features.WeatherForecast;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using CleanArchitecture.WebUI.Features.Home;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.WebUI.Models;

namespace WebUI.UnitTests;
public class WhenHome
{
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _sut;

    public WhenHome()
    {
        _mockHttpContext = new Mock<HttpContext>();
        var controllerCtx = new ControllerContext
        {
            HttpContext = _mockHttpContext.Object
        };
        _mockLogger = new Mock<ILogger<HomeController>>();
        _sut = new HomeController(_mockLogger.Object)
        {
            ControllerContext = controllerCtx
        };

    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.

    [Fact]
    public void ThenTheIndexMethodIsCalled_ViewResultReturned()
    {

        //Act
        var viewResult = _sut.Index() as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();

    }

    [Fact]
    public void ThenThePrivacyMethodIsCalled_ViewResultReturned()
    {

        //Act
        var viewResult = _sut.Privacy() as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();

    }

    [Fact]
    public void ThenTheErrorMethodIsCalled_ViewResultReturned()
    {
        //Arrange
        ErrorViewModel viewModel = new ErrorViewModel()
        {
            RequestId = null
        };

        //Act
        var viewResult = _sut.Error() as ViewResult;

        //Assert
        viewResult.Should().NotBeNull();
        var model = viewResult.Model as ErrorViewModel;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(viewModel);

    }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
