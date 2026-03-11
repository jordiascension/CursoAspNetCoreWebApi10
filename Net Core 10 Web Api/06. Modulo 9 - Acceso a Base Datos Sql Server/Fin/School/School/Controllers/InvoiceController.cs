using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using School.Apis.Contracts;
using School.Application.Contracts;

namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoicingAppService _appService;

        public InvoiceController(IInvoicingAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto.CreateInvoiceResponse>> Create(
                    InvoiceDto.CreateInvoiceRequest request,
                    CancellationToken ct)
        {
            var result = await _appService.CreateInvoiceAsync(request, ct);
            return Ok(result);
        }
    }
}
