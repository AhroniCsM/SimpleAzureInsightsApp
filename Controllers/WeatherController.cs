using Microsoft.AspNetCore.Mvc;
using SimpleAzureInsightsApp.Services;
using System.Diagnostics;

namespace SimpleAzureInsightsApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ITraceService _traceService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(ITraceService traceService, ILogger<WeatherController> logger)
    {
        _traceService = traceService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeather()
    {
        using var activity = Activity.StartActivity("GetWeather");
        activity?.SetTag("operation", "get_weather");
        activity?.SetTag("controller", "WeatherController");

        _logger.LogInformation("Getting weather forecast");

        try
        {
            // Simulate some work
            await Task.Delay(100);

            // Generate some traces
            await _traceService.GenerateSampleTraces();

            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = GetRandomSummary()
            }).ToArray();

            _logger.LogInformation("Weather forecast generated successfully with {Count} items", weather.Length);
            activity?.SetTag("items_count", weather.Length.ToString());

            return Ok(weather);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting weather forecast");
            activity?.SetTag("error", true);
            activity?.SetTag("error_message", ex.Message);
            throw;
        }
    }

    [HttpGet("error")]
    public IActionResult GetError()
    {
        using var activity = Activity.StartActivity("GetError");
        activity?.SetTag("operation", "get_error");
        activity?.SetTag("controller", "WeatherController");

        _logger.LogWarning("Simulating an error for testing purposes");

        try
        {
            throw new InvalidOperationException("This is a simulated error for testing Application Insights");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Simulated error occurred");
            activity?.SetTag("error", true);
            activity?.SetTag("error_message", ex.Message);
            return StatusCode(500, new { error = ex.Message });
        }
    }

    private static string GetRandomSummary()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        return summaries[Random.Shared.Next(summaries.Length)];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
