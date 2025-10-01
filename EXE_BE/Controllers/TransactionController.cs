using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EXE_BE.Services;
using EXE_BE.Controllers.ViewModel;
using Net.payOS.Types;
using Microsoft.AspNetCore.Authorization;

namespace EXE_BE.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _transactionService.CreateTransactionAsync(request);
            
            if (result.Success)
                return CreatedAtAction(nameof(GetTransactionById), new { id = result.Data!.Id }, result);
            
            return BadRequest(result);
        }

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id);
            
            if (result.Success)
                return Ok(result);
            
            return NotFound(result);
        }

        /// <summary>
        /// Get all transactions with filtering and pagination
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionFilterRequest filter)
        {
            var result = await _transactionService.GetAllTransactionsAsync(filter);
            
            if (result.Success)
                return Ok(result);
            
            return BadRequest(result);
        }

        /// <summary>
        /// Get transactions by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(int userId)
        {
            var result = await _transactionService.GetTransactionsByUserIdAsync(userId);
            
            if (result.Success)
                return Ok(result);
            
            return BadRequest(result);
        }

        /// <summary>
        /// Update an existing transaction
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _transactionService.UpdateTransactionAsync(id, request);
            
            if (result.Success)
                return Ok(result);
            
            return NotFound(result);
        }

        /// <summary>
        /// Delete a transaction
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);
            
            if (result.Success)
                return Ok(result);
            
            return NotFound(result);
        }

        /// <summary>
        /// Handle PayOS payment return webhook
        /// </summary>
        [HttpPost("payment-return")]
        public async Task<IActionResult> PaymentReturn([FromBody] WebhookType request)
        {
            await _transactionService.HandlePayOSReturnAsync(request);
            return Ok();
        }
    }
}
