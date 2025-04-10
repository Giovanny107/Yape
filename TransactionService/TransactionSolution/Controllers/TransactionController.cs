using Microsoft.AspNetCore.Mvc;
using Transaction.Application.DTOs;
using Transaction.Application.Interfaces;

namespace TransactionSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        private readonly ITransactionService _transactionService = transactionService;

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            var transaction = await _transactionService.CreateTransactionAsync(request);
            return CreatedAtAction("CreateTransaction", new { transactionId = transaction.TransactionExternalId }, transaction);
        }
    }
}
