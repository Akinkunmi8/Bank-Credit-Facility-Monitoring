using BankLoanMonetrySolu.IServices;
using Contracts.Dto;
using Contracts.Publishing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankLoanMonetrySolu.Services
{
    public class TATMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TATMonitorService> _logger;

        public TATMonitorService(IServiceScopeFactory scopeFactory, ILogger<TATMonitorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TAT Monitor Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Starting TAT check cycle at {Time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var tatService = scope.ServiceProvider.GetRequiredService<ITAT>();
                        var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                        var regions = new[] { "West", "East", "North", "South" };

                        foreach (var region in regions)
                        {
                            try
                            {
                                var appIds = await tatService.GetBeyondTATAppIds(region);

                                if (appIds.Any())
                                {
                                    _logger.LogInformation("Region {Region}: Found {Count} applications beyond TAT.", region, appIds.Count());

                                    foreach (var appId in appIds)
                                    {
                                        await publisher.PublishAsync(new TATEvent(appId));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing TAT for region: {Region}", region);
                                // Continue to the next region even if one fails
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "An unhandled exception occurred in the TAT Monitor Service cycle.");
                }

                _logger.LogDebug("TAT cycle completed. Sleeping for 10 minutes.");

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("TAT Monitor Service is stopping due to task cancellation.");
                }
            }
        }
    }
}