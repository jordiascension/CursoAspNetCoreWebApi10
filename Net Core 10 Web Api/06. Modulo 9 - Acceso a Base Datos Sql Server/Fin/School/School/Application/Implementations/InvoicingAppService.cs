using School.Application.Contracts;
using School.Domain.Contracts;
using School.Models;

using static School.Apis.Contracts.InvoiceDto;

namespace School.Application.Implementations
{
    public class InvoicingAppService : IInvoicingAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoicingDomainService _domain;

        public InvoicingAppService(IUnitOfWork uow, IInvoicingDomainService domain)
        {
            _uow = uow;
            _domain = domain;
        }

        public async Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken ct = default)
        {
            if (request.Lines is null || request.Lines.Count == 0)
                throw new ArgumentException("Invoice must contain at least one line.");

            var domainLines = request.Lines.Select(l =>
                new NewInvoiceLine(l.Description, l.Quantity, l.UnitPrice, l.VatRate));

            var invoice = _domain.CreateInvoice(request.Date, domainLines);

            await _uow.BeginTransactionAsync(ct);
            try
            {
                var invoiceRepo = _uow.Repository<Invoice>();

                // With proper EF Core relationship mapping, this persists Invoice + Lines in one go.
                await invoiceRepo.AddAsync(invoice, ct);

                await _uow.SaveChangesAsync(ct);
                await _uow.CommitTransactionAsync(ct);

                return new CreateInvoiceResponse(invoice.Id, invoice.TaxableBase, invoice.TotalVat, invoice.Total);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
