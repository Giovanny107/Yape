using Transaction.Application.Interfaces;
using Transaction.Domain.Entities;
using Transaction.Domain.Enums;
using Transaction.Domain.Interfaces;

namespace Transaction.Application.Services
{
    public class ValidationResultService(ITransactionRepository repository) : IValidationResultService
    {
        private readonly ITransactionRepository _repository = repository;

        public async Task ProcessValidationResult(ValidationResult validationResult)
        {
            var transaction = await _repository.GetTransactionAsync(validationResult.Id) ?? throw new Exception("Transaction not found");
            transaction.Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), validationResult.Status);
            await _repository.UpdateAsync(transaction);
        }
    }
}
