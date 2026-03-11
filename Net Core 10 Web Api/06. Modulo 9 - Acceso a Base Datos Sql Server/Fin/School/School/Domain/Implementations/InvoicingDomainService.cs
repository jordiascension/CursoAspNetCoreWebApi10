using School.Domain.Contracts;
using School.Models;

namespace School.Domain.Implementations
{
    public class InvoicingDomainService : IInvoicingDomainService
    {
        public Invoice CreateInvoice(DateTime date, IEnumerable<NewInvoiceLine> lines)
        {
            var invoice = new Invoice(date);

            foreach (var l in lines)
                invoice.AddLine(l.Description, l.Quantity, l.UnitPrice, l.VatRate);

            return invoice;
        }
    }
}
