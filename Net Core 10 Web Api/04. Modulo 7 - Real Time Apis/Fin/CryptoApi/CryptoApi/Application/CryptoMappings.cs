using CriptoApi.Domain;

namespace CriptoApi.Application
{
    // Extension methods to convert Domain entities into Application DTOs.
    public static class CryptoMappings
    {
        public static CryptoPriceItem ToPriceItem(this Crypto crypto, DateTime utcNow) =>
       new()
       {
           Id = crypto.Id,
           Symbol = crypto.Symbol,
           Name = crypto.Name,
           PriceUsd = crypto.PriceUsd,
           LastUpdatedUtc = utcNow
       };
    }
}
