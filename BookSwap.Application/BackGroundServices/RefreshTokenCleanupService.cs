// في BookExchange.Infrastructure/Services/RefreshTokenCleanupService.cs
using BookExchange.Infrastructure.Services;
using BookSwap.Infrastructure.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookExchange.Infrastructure.Services
{
    public class RefreshTokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RefreshTokenCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(6);

        public RefreshTokenCleanupService(IServiceProvider serviceProvider, ILogger<RefreshTokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Refresh Token Cleanup Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepositoryAsync>();

                    await refreshTokenRepository.CleanExpiredRefreshTokensAsync();
                    _logger.LogInformation("Expired refresh tokens cleaned successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning expired refresh tokens.");
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Refresh Token Cleanup Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}

