using Microsoft.AspNetCore.Mvc;

namespace KonaAI.Master.API.Controllers;

/// <summary>
/// Provides simple weather forecast data for testing and sample purposes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// Returns a list of sample weather forecasts.
    /// </summary>
    /// <returns>200 OK with a collection of <see cref="WeatherForecast"/>.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        logger.LogInformation("WeatherForecastController: Get - execution started");

        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });

        logger.LogInformation("WeatherForecastController: Get - returning {Count} records", forecasts.Count());
        return Ok(forecasts);
    }
}