using CriptoApi.Domain;

namespace CriptoApi.Infrastructure
{
    // In-memory implementation of the crypto repository.
    // Useful for demos, development, or unit testing
    public class InMemoryCryptoRepository : ICryptoRepository
    {
        private readonly List<Crypto> _cryptos;

        public InMemoryCryptoRepository()
        {
            // Predefined static list of cryptocurrencies.
            _cryptos = new List<Crypto>
            {
                new("bitcoin",   "BTC",  "Bitcoin",   45000m),
                new("ethereum",  "ETH",  "Ethereum",  3000m),
                new("solana",    "SOL",  "Solana",    150m),
                new("ripple",    "XRP",  "Ripple",    0.7m),
                new("cardano",   "ADA",  "Cardano",   0.4m),
                new("dogecoin",  "DOGE", "Dogecoin",  0.09m),
                new("polkadot",  "DOT",  "Polkadot",  8m),
                new("litecoin",  "LTC",  "Litecoin",  100m),
                new("tron",      "TRX",  "Tron",      0.1m),
                new("chainlink", "LINK", "Chainlink", 18m),
            };
        }

        public IReadOnlyCollection<Crypto> GetAll() => _cryptos.AsReadOnly();

        public Crypto? GetById(string id) =>
            _cryptos.FirstOrDefault(c => c.Id == id);
    }
}
