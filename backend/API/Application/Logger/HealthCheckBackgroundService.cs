using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HealthCheckBackgroundService> _logger;
    private readonly TimeSpan _delay;
    private readonly TimeSpan _period;

    // Armazena o último status geral do health check
    private HealthStatus? _lastStatus = null;

    public HealthCheckBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<HealthCheckBackgroundService> logger,
        IOptions<HealthCheckPublisherOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _delay = options.Value.Delay;
        _period = options.Value.Period;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Delay inicial antes da primeira execução
        await Task.Delay(_delay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
                var report = await healthCheckService.CheckHealthAsync(stoppingToken);

                // Só publica/loga se o status geral mudou desde a última execução
                if (_lastStatus != report.Status)
                {
                    var publishers = scope.ServiceProvider.GetServices<IHealthCheckPublisher>();
                    foreach (var publisher in publishers)
                    {
                        await publisher.PublishAsync(report, stoppingToken);
                    }

                    _logger.LogInformation("Health Check status mudou: {Status}", report.Status);
                    _lastStatus = report.Status;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar Health Check Publisher.");
            }

            await Task.Delay(_period, stoppingToken);
        }
    }
}
