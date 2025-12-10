namespace CriptoApi.Application
{
    using CriptoApi.Domain;
    using System.Collections.Concurrent;

    // Core Application service responsible for:
    // - deciding when each crypto should update
    // - applying price variations
    // - producing output DTOs for the API layers
    public class CryptoPriceService : ICryptoPriceService
    {
        private readonly ICryptoRepository _repository;
        private readonly Random _random = new();

        private static readonly Dictionary<string, TimeSpan> UpdateIntervals =
            new()
            {
                ["bitcoin"] = TimeSpan.FromSeconds(1),
                ["ethereum"] = TimeSpan.FromSeconds(2),
                ["solana"] = TimeSpan.FromSeconds(3),
                ["ripple"] = TimeSpan.FromSeconds(4),
                ["cardano"] = TimeSpan.FromSeconds(5),
                ["dogecoin"] = TimeSpan.FromSeconds(1.5),
                ["polkadot"] = TimeSpan.FromSeconds(2.5),
                ["litecoin"] = TimeSpan.FromSeconds(3.5),
                ["tron"] = TimeSpan.FromSeconds(4.5),
                ["chainlink"] = TimeSpan.FromSeconds(6),
            };

        // Tracks the next update time for each crypto.
        private readonly ConcurrentDictionary<string, DateTime> _nextUpdateUtc = new();

        public CryptoPriceService(ICryptoRepository repository)
        {
            _repository = repository;

            var now = DateTime.UtcNow;
            // Initialize schedule for each crypto.
            foreach (var c in _repository.GetAll())
            {
                _nextUpdateUtc[c.Id] = now + UpdateIntervals[c.Id];
            }
        }

        public Task<GetCryptoPricesResponse> GetCurrentPricesAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var items = _repository.GetAll()
                .Select(c => c.ToPriceItem(now))
                .ToList();

            return Task.FromResult(new GetCryptoPricesResponse
            {
                Prices = items
            });
        }

        public Task<CryptoPricesUpdatedMessage?> GetDueUpdatesAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var updated = new List<CryptoPriceItem>();

            foreach (var crypto in _repository.GetAll())
            {
                // Skip cryptos that are not due for update.
                if (now < _nextUpdateUtc[crypto.Id])
                    continue;

                // Apply pricing variation.
                crypto.ApplyRandomVariation(-0.02m, 0.02m, _random);

                // Compute next scheduled update.
                _nextUpdateUtc[crypto.Id] = now + UpdateIntervals[crypto.Id];

                updated.Add(crypto.ToPriceItem(now));
            }

            // No updates → return null to avoid unnecessary SignalR traffic.
            if (updated.Count == 0)
                return Task.FromResult<CryptoPricesUpdatedMessage?>(null);

            return Task.FromResult<CryptoPricesUpdatedMessage?>(
                new CryptoPricesUpdatedMessage { Updates = updated }
            );
        }
    }

}
