namespace CriptoApi.Application
{
    // SignalR message broadcasted when one or more
    // cryptocurrencies update their price.
    public record CryptoPricesUpdatedMessage
    {
        public IReadOnlyCollection<CryptoPriceItem> Updates { get; init; }
            = Array.Empty<CryptoPriceItem>();
    }
}
