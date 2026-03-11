using School.Models;

namespace School.Domain.Contracts
{
    public interface IInvoicingDomainService
    {
        Invoice CreateInvoice(DateTime date, IEnumerable<NewInvoiceLine> lines);
    }

    public record NewInvoiceLine(string Description, decimal Quantity, 
                                 decimal UnitPrice, VatRate VatRate);
}
