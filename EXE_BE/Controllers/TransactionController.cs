using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EXE_BE.Services;
using EXE_BE.Controllers.ViewModel;
using Net.payOS.Types;

namespace EXE_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("payment-return")]
        public async Task<IActionResult> PaymentReturn([FromBody] WebhookType request)
        {
            await _transactionService.HandlePayOSReturnAsync(request);

            return Ok();
        }
    }
}
