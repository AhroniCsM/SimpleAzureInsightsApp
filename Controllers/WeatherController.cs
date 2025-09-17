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

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        using var activity = Activity.StartActivity("GetStatus");
        activity?.SetTag("operation", "get_status");
        activity?.SetTag("controller", "WeatherController");

        _logger.LogInformation("Application status requested");

        var status = new
        {
            Application = "SimpleAzureInsightsApp",
            Status = "Running",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            MachineName = Environment.MachineName,
            ProcessId = Environment.ProcessId
        };

        activity?.SetTag("status", "healthy");
        activity?.SetTag("machine_name", Environment.MachineName);

        _logger.LogInformation("Application status: {Status}", status.Status);

        return Ok(status);
    }

    [HttpPost("generate-traces")]
    public async Task<IActionResult> GenerateTraces()
    {
        using var activity = Activity.StartActivity("GenerateTraces");
        activity?.SetTag("operation", "generate_traces");
        activity?.SetTag("controller", "WeatherController");

        _logger.LogInformation("Manual trace generation requested");

        try
        {
            await _traceService.GenerateSampleTraces();

            // Generate additional traces
            for (int i = 0; i < 5; i++)
            {
                using var traceActivity = Activity.StartActivity($"ManualTrace_{i}");
                traceActivity?.SetTag("trace_number", i.ToString());
                traceActivity?.SetTag("source", "manual_request");

                _logger.LogInformation("Generated manual trace {TraceNumber}", i);

                // Simulate some work
                await Task.Delay(100);
            }

            _logger.LogInformation("Manual trace generation completed successfully");
            activity?.SetTag("traces_generated", "5");

            return Ok(new { message = "Traces generated successfully", count = 5, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during manual trace generation");
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
