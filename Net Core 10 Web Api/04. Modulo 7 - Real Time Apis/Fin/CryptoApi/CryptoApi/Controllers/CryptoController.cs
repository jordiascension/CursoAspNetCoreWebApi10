using CriptoApi.Application;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CriptoApi.Controllers
{

    // REST controller that exposes the current price list.
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoPriceService _cryptoPriceService;

        public CryptoController(ICryptoPriceService cryptoPriceService)
        {
            _cryptoPriceService = cryptoPriceService;
        }

        // GET /api/crypto
        // Returns a snapshot of all crypto prices.
        [HttpGet]
        public async Task<ActionResult<GetCryptoPricesResponse>> GetAll(CancellationToken ct)
        {
            var response = await _cryptoPriceService.GetCurrentPricesAsync(ct);
            return Ok(response); // <- Response, no DTO, no dominio  
        }
    }
}
