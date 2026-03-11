using static School.Apis.Contracts.InvoiceDto;

namespace School.Application.Contracts
{
    public interface IInvoicingAppService
    {
        Task<CreateInvoiceResponse> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken ct = default);
    }
}
