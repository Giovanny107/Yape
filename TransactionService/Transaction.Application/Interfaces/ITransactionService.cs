using Transaction.Application.DTOs;

namespace Transaction.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request);
    }
}
