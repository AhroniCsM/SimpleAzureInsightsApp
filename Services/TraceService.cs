using System.Diagnostics;

namespace SimpleAzureInsightsApp.Services;

public class TraceService : ITraceService
{
    private readonly ILogger<TraceService> _logger;

    public TraceService(ILogger<TraceService> logger)
    {
        _logger = logger;
    }

    public async Task GenerateSampleTraces()
    {
        using var activity = Activity.StartActivity("GenerateSampleTraces");
        activity?.SetTag("service", "TraceService");
        activity?.SetTag("operation", "generate_traces");

        _logger.LogInformation("Starting to generate sample traces");

        // Simulate some business logic with traces
        await ProcessUserData();
        await ValidateData();
        await SaveData();

        _logger.LogInformation("Sample traces generation completed");
    }

    private async Task ProcessUserData()
    {
        using var activity = Activity.StartActivity("ProcessUserData");
        activity?.SetTag("step", "process_user_data");

        _logger.LogDebug("Processing user data");

        // Simulate processing time
        await Task.Delay(50);

        _logger.LogDebug("User data processed successfully");
    }

    private async Task ValidateData()
    {
        using var activity = Activity.StartActivity("ValidateData");
        activity?.SetTag("step", "validate_data");

        _logger.LogDebug("Validating data");

        // Simulate validation time
        await Task.Delay(30);

        _logger.LogDebug("Data validation completed");
    }

    private async Task SaveData()
    {
        using var activity = Activity.StartActivity("SaveData");
        activity?.SetTag("step", "save_data");

        _logger.LogDebug("Saving data");

        // Simulate save operation
        await Task.Delay(20);

        _logger.LogDebug("Data saved successfully");
    }
}
