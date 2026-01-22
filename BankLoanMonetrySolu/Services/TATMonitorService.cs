using BankLoanMonetrySolu.IServices;
using Contracts.Dto;
using Contracts.Publishing;

namespace BankLoanMonetrySolu.Services
{
    public class TATMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TATMonitorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var tatService = scope.ServiceProvider.GetRequiredService<ITAT>();
                var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

                var regions = new[] { "West", "East", "North", "South" };

                foreach (var region in regions)
                {
                    var appIds = await tatService.GetBeyondTATAppIds(region);

                    foreach (var appId in appIds)
                    {
                        await publisher.PublishAsync(new TATEvent(appId));
                    }
                }

                // run every 10 minutes
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }

}
