namespace CriptoApi.Domain
{
    // Domain entity that represents a cryptocurrency.
    // Contains business logic related to pricing behavior.
    public class Crypto
    {
        public string Id { get; }
        public string Symbol { get; }
        public string Name { get; }

        private decimal _priceUsd;
        public decimal PriceUsd => _priceUsd;

        public Crypto(string id, string symbol, string name, decimal initialPrice)
        {
            Id = id;
            Symbol = symbol;
            Name = name;
            _priceUsd = initialPrice;
        }

        // Applies a random variation to the crypto price.
        // This simulates real market fluctuations.
        public void ApplyRandomVariation(decimal minPercent, decimal maxPercent, Random random)
        {
            // Pick a random variation percentage within the configured range.
            var range = (double)(maxPercent - minPercent);
            var value = (decimal)(random.NextDouble() * range) + minPercent;

            // Calculate updated price.
            var newPrice = _priceUsd * (1 + value);

            // Prevent negative or zero prices.
            if (newPrice <= 0)
                return;

            _priceUsd = decimal.Round(newPrice, 2);
        }
    }
}
