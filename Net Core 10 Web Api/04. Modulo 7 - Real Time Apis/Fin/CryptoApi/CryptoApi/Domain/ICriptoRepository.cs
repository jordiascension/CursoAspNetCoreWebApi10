namespace CriptoApi.Domain
{
    // Repository contract for accessing cryptocurrency data.
    // The Application layer relies only on this abstraction,
    // without knowing how data is persisted or stored.
    public interface ICryptoRepository
    {
        IReadOnlyCollection<Crypto> GetAll();
        Crypto? GetById(string id);
    }
}
