namespace CriptoApi.Application
{
    // Application service that coordinates crypto updates
    // and exposes formatted data for REST and SignalR.
    public interface ICryptoPriceService
    {
        Task<GetCryptoPricesResponse> GetCurrentPricesAsync(CancellationToken ct = default);
        Task<CryptoPricesUpdatedMessage?> GetDueUpdatesAsync(CancellationToken ct = default);
    }
}
