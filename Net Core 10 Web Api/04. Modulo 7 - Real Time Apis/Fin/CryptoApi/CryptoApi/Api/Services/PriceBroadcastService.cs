namespace CriptoApi.Api.Services
{
    using CriptoApi.Api.Hubs;
    using CriptoApi.Application;
    // Api/Services/PriceBroadcastService.cs
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    // Background service that periodically (200ms) checks which cryptos
    // require updates and broadcasts them via SignalR.
    public class PriceBroadcastService : BackgroundService
    {
        private readonly ICryptoPriceService _priceService;
        private readonly IHubContext<CryptoHub> _hubContext;
        private readonly ILogger<PriceBroadcastService> _logger;

        public PriceBroadcastService(
            ICryptoPriceService priceService,
            IHubContext<CryptoHub> hubContext,
            ILogger<PriceBroadcastService> logger)
        {
            _priceService = priceService;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Ask the Application layer for cryptos that need updating.
                    var message = await _priceService.GetDueUpdatesAsync(stoppingToken);

                    if (message is not null)
                    {
                        // Only broadcast actual updates.
                        await _hubContext.Clients.All
                            .SendAsync("CryptoPricesUpdated", message, cancellationToken: stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error emitiendo actualizaciones de criptos.");
                }

                await Task.Delay(200, stoppingToken);
            }
        }
    }

}
