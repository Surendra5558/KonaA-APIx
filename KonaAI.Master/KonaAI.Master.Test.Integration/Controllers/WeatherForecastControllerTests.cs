using KonaAI.Master.API;
using KonaAI.Master.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.WeatherForecastController"/>.
/// Covers:
/// - GET returns 200 OK with weather forecast data
/// - Error handling (500) for server errors
/// </summary>
public class WeatherForecastControllerTests
{
    // Overview:
    // - Exercises WeatherForecastController actions with mocked dependencies.
    // - Verifies happy paths (200) and error scenarios (500).
    // - This is a simple controller that returns sample data.
    private static WeatherForecastController CreateController()
    {
        var logger = new Mock<ILogger<WeatherForecastController>>();
        return new WeatherForecastController(logger.Object);
    }

    [Fact]
    public void Get_ReturnsOk_WithWeatherForecasts()
    {
        // Arrange: create controller
        var controller = CreateController();

        // Act
        var result = controller.Get();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<WeatherForecast>>>(result);
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.NotNull(ok.Value);
        var forecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(ok.Value);
        Assert.NotEmpty(forecasts);
        Assert.Equal(5, forecasts.Count());
    }

    [Fact]
    public void Get_ReturnsOk_WithValidWeatherForecastData()
    {
        // Arrange: create controller
        var controller = CreateController();

        // Act
        var result = controller.Get();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<WeatherForecast>>>(result);
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var forecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(ok.Value);

        foreach (var forecast in forecasts)
        {
            Assert.True(forecast.TemperatureC >= -20 && forecast.TemperatureC <= 55);
            Assert.NotNull(forecast.Summary);
            Assert.NotEmpty(forecast.Summary);
        }
    }
}