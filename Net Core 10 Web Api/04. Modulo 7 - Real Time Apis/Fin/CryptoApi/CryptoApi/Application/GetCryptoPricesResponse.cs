namespace CriptoApi.Application
{
    // Response returned by GET /api/crypto.
    public record GetCryptoPricesResponse
    {
        public IReadOnlyCollection<CryptoPriceItem> Prices { get; init; }
       = Array.Empty<CryptoPriceItem>();
    }
}
