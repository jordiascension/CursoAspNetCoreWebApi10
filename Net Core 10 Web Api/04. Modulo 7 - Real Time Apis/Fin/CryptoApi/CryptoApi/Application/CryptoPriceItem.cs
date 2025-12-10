namespace CriptoApi.Application
{
    // Data Transfer Object used by REST and SignalR.
    // Contains only data — no business logic.    
    public record CryptoPriceItem
    {
        public string Id { get; init; } = default!;
        public string Symbol { get; init; } = default!;
        public string Name { get; init; } = default!;
        public decimal PriceUsd { get; init; }
        public DateTime LastUpdatedUtc { get; init; }
    }

}
