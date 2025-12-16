using CryptoClient.Models;

using Microsoft.AspNetCore.SignalR.Client;

using System.Net.Http.Json;

namespace CryptoClient.Services
{
    public class CryptoService : Contracts.ICryptoService, IAsyncDisposable
    {
        private readonly IHttpClientFactory _httpFactory;
        private HubConnection? _hubConnection;

        private const string ApiBaseUrl = "https://localhost:7037/";

        private readonly Dictionary<string, CryptoPriceItem> _cryptoRegistry = new();
        private List<CryptoPriceItem> _currentPrices = new();

        public IReadOnlyList<CryptoPriceItem> CurrentPrices => _currentPrices;

        public event Action? PricesUpdated;

        public CryptoService(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<IReadOnlyList<CryptoPriceItem>> GetInitialPricesAsync()
        {
            var client = _httpFactory.CreateClient("CryptoApi");

            var response = await client.GetFromJsonAsync<GetCryptoPricesResponse>("api/crypto");
            var CryptoPriceItemList = response?.Prices ?? new();

            _cryptoRegistry.Clear();
            foreach (var CryptoPriceItem in CryptoPriceItemList)
            {
                CryptoPriceItem.BaselinePriceUsd = CryptoPriceItem.PriceUsd;
                _cryptoRegistry[CryptoPriceItem.Id] = CryptoPriceItem;
            }

            RebuildCurrentPrices();

            await StartSignalRAsync();

            return CurrentPrices;
        }

        private async Task StartSignalRAsync()
        {
            if (_hubConnection != null) return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(ApiBaseUrl), "/hubs/crypto"))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<CryptoPricesUpdatedMessage>("CryptoPricesUpdated", HandleUpdates);

            await _hubConnection.StartAsync();
        }

        private void HandleUpdates(CryptoPricesUpdatedMessage message)
        {
            if (message?.Updates == null || message.Updates.Count == 0) return;

            var changed = false;

            foreach (var update in message.Updates)
            {
                if (_cryptoRegistry.TryGetValue(update.Id, out var existing))
                {
                    if (existing.PriceUsd != update.PriceUsd)
                    {
                        existing.PriceUsd = update.PriceUsd;
                        changed = true;
                    }
                }
                else
                {
                    update.BaselinePriceUsd = update.PriceUsd;
                    _cryptoRegistry[update.Id] = update;
                    changed = true;
                }
            }

            if (changed)
            {
                RebuildCurrentPrices();
                PricesUpdated?.Invoke();
            }
        }

        private void RebuildCurrentPrices()
        {
            _currentPrices = _cryptoRegistry.Values.ToList();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is null) return;

            try
            {
                await _hubConnection.StopAsync();
            }
            finally
            {
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }

        private class GetCryptoPricesResponse
        {
            public List<CryptoPriceItem> Prices { get; set; } = new();
        }

        private class CryptoPricesUpdatedMessage
        {
            public List<CryptoPriceItem> Updates { get; set; } = new();
        }
    }
}
