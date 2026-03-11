using School.Models;

namespace School.Apis.Contracts
{
    public class InvoiceDto
    {
        // CREATE
        // ======================

        public record CreateInvoiceRequest(DateTime Date, 
            List<CreateInvoiceLineRequest> Lines);

        public record CreateInvoiceLineRequest(string Description, 
            decimal Quantity, decimal UnitPrice, VatRate VatRate);

        public record CreateInvoiceResponse(int InvoiceId,
            decimal TaxableBase, decimal TotalVat, decimal Total);

        // ======================
        // READ
        // ======================

        public record Response(
            int Id,
            DateTime Date,
            decimal TaxableBase,
            decimal TotalVat,
            decimal Total,
            List<LineResponse> Lines
        );

        public record LineResponse(
            int Id,
            string Description,
            decimal Quantity,
            decimal UnitPrice,
            VatRate VatRate,
            decimal LineBase,
            decimal VatAmount,
            decimal LineTotal
        );
    }
}
