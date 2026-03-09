namespace School.Models
{
    public class Invoice
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }

        public decimal TaxableBase { get; private set; }  // Base imponible
        public decimal TotalVat { get; private set; }
        public decimal Total { get; private set; }

        private readonly List<InvoiceLine> _lines = new();
        public IReadOnlyCollection<InvoiceLine> Lines => _lines.AsReadOnly();

        private Invoice() { } // EF Core

        public Invoice(DateTime date)
        {
            Date = date;
        }

        public void AddLine(string description, decimal quantity, 
                            decimal unitPrice, VatRate vatRate)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (unitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            var line = InvoiceLine.Create(description, quantity, unitPrice, vatRate);
            _lines.Add(line);

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            TaxableBase = _lines.Sum(l => l.LineBase);
            TotalVat = _lines.Sum(l => l.VatAmount);
            Total = TaxableBase + TotalVat;
        }
    }
}
