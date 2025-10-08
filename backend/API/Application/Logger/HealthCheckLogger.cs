using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HealthCheckLogger : IHealthCheckPublisher
{
    private readonly ILogger<HealthCheckLogger> _logger;

    public HealthCheckLogger(ILogger<HealthCheckLogger> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        foreach (var entry in report.Entries)
        {
            if (entry.Value.Status != HealthStatus.Healthy)
            {
                _logger.LogWarning("⚠ Health Check failed: {Key} - Status: {Status} - Description: {Description}",
                    entry.Key, entry.Value.Status, entry.Value.Description ?? "No description");
            }
            else
            {
                _logger.LogInformation("✔ Health Check OK: {Key}", entry.Key);
            }
        }

        return Task.CompletedTask;
    }
}