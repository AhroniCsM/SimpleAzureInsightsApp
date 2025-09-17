using System.Diagnostics;

namespace SimpleAzureInsightsApp.Services;

public class BackgroundTraceService : BackgroundService
{
    private readonly ILogger<BackgroundTraceService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundTraceService(ILogger<BackgroundTraceService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background trace service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var activity = Activity.StartActivity("BackgroundTraceGeneration");
                activity?.SetTag("service", "BackgroundTraceService");
                activity?.SetTag("operation", "continuous_trace_generation");

                _logger.LogInformation("Generating background traces at {Time}", DateTime.UtcNow);

                // Generate different types of traces
                await GenerateBusinessTraces();
                await GenerateSystemMetrics();
                await GenerateUserActivity();

                _logger.LogInformation("Background traces generated successfully");

                // Wait for 30 seconds before next generation
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Background trace service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in background trace service");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        _logger.LogInformation("Background trace service stopped");
    }

    private async Task GenerateBusinessTraces()
    {
        using var activity = Activity.StartActivity("GenerateBusinessTraces");
        activity?.SetTag("trace_type", "business");
        activity?.SetTag("category", "background");

        _logger.LogDebug("Generating business traces");

        // Simulate business operations
        var operations = new[]
        {
            "ProcessOrder",
            "ValidatePayment",
            "UpdateInventory",
            "SendNotification",
            "GenerateReport"
        };

        foreach (var operation in operations)
        {
            using var opActivity = Activity.StartActivity(operation);
            opActivity?.SetTag("operation", operation);
            opActivity?.SetTag("trace_type", "business");

            _logger.LogInformation("Executing business operation: {Operation}", operation);

            // Simulate processing time
            await Task.Delay(Random.Shared.Next(100, 500));

            // Simulate occasional errors
            if (Random.Shared.Next(1, 20) == 1)
            {
                _logger.LogWarning("Simulated error in operation: {Operation}", operation);
                opActivity?.SetTag("error", true);
                opActivity?.SetTag("error_message", "Simulated business error");
            }
            else
            {
                _logger.LogDebug("Operation {Operation} completed successfully", operation);
            }
        }
    }

    private async Task GenerateSystemMetrics()
    {
        using var activity = Activity.StartActivity("GenerateSystemMetrics");
        activity?.SetTag("trace_type", "system");
        activity?.SetTag("category", "background");

        _logger.LogDebug("Generating system metrics");

        // Simulate system metrics
        var cpuUsage = Random.Shared.Next(20, 90);
        var memoryUsage = Random.Shared.Next(30, 85);
        var diskUsage = Random.Shared.Next(40, 95);

        activity?.SetTag("cpu_usage", cpuUsage.ToString());
        activity?.SetTag("memory_usage", memoryUsage.ToString());
        activity?.SetTag("disk_usage", diskUsage.ToString());

        _logger.LogInformation("System metrics - CPU: {Cpu}%, Memory: {Memory}%, Disk: {Disk}%", 
            cpuUsage, memoryUsage, diskUsage);

        // Simulate processing time
        await Task.Delay(Random.Shared.Next(50, 200));

        // Log warnings for high usage
        if (cpuUsage > 80)
        {
            _logger.LogWarning("High CPU usage detected: {Cpu}%", cpuUsage);
        }
        if (memoryUsage > 80)
        {
            _logger.LogWarning("High memory usage detected: {Memory}%", memoryUsage);
        }
    }

    private async Task GenerateUserActivity()
    {
        using var activity = Activity.StartActivity("GenerateUserActivity");
        activity?.SetTag("trace_type", "user_activity");
        activity?.SetTag("category", "background");

        _logger.LogDebug("Generating user activity traces");

        // Simulate user activities
        var activities = new[]
        {
            "UserLogin",
            "ViewProduct",
            "AddToCart",
            "Checkout",
            "Logout"
        };

        var userId = Random.Shared.Next(1000, 9999);
        activity?.SetTag("user_id", userId.ToString());

        foreach (var userActivity in activities)
        {
            using var userActivitySpan = Activity.StartActivity(userActivity);
            userActivitySpan?.SetTag("user_id", userId.ToString());
            userActivitySpan?.SetTag("activity", userActivity);

            _logger.LogInformation("User {UserId} performed activity: {Activity}", userId, userActivity);

            // Simulate processing time
            await Task.Delay(Random.Shared.Next(50, 300));

            // Simulate occasional user errors
            if (Random.Shared.Next(1, 15) == 1)
            {
                _logger.LogWarning("User {UserId} encountered error in activity: {Activity}", userId, userActivity);
                userActivitySpan?.SetTag("error", true);
                userActivitySpan?.SetTag("error_message", "Simulated user activity error");
            }
        }
    }
}
