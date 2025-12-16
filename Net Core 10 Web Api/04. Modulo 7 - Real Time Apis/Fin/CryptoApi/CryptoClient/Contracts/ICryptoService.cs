using CryptoClient.Models;

namespace CryptoClient.Contracts
{
    public interface ICryptoService
    {
        Task<IReadOnlyList<CryptoPriceItem>> GetInitialPricesAsync();

        event Action? PricesUpdated;

        IReadOnlyList<CryptoPriceItem> CurrentPrices { get; }
    }
}
