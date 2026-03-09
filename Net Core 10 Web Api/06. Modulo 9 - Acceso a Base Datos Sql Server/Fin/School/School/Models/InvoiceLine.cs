namespace School.Models
{
    public class InvoiceLine
    {
        public int Id { get; private set; }

        public int InvoiceId { get; private set; }
        public Invoice Invoice { get; private set; } = null!;

        public string Description { get; private set; } = null!;
        public decimal Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public VatRate VatRate { get; private set; }

        public decimal LineBase { get; private set; }   // quantity * unitPrice
        public decimal VatAmount { get; private set; }
        public decimal LineTotal { get; private set; }  // base + vat

        private InvoiceLine() { } // EF Core

        public static InvoiceLine Create(string description,
                                  decimal quantity, decimal unitPrice,
                                  VatRate vatRate)
        {
            var lineBase = Round2(quantity * unitPrice);
            var vatAmount = Round2(lineBase * ((int)vatRate / 100m));
            var total = lineBase + vatAmount;

            return new InvoiceLine
            {
                Description = description.Trim(),
                Quantity = quantity,
                UnitPrice = unitPrice,
                VatRate = vatRate,
                LineBase = lineBase,
                VatAmount = vatAmount,
                LineTotal = total
            };
        }

        private static decimal Round2(decimal value)
            => Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
