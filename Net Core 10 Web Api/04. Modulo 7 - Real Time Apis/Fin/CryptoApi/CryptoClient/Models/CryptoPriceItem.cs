namespace CryptoClient.Models
{
    public class CryptoPriceItem
    {
        public string Id { get; init; } = default!;
        public string Symbol { get; init; } = default!;
        public string Name { get; init; } = default!;
        public decimal PriceUsd { get; set; }
        public DateTime LastUpdatedUtc { get; init; }
        public decimal BaselinePriceUsd { get; set; }
    }
}
