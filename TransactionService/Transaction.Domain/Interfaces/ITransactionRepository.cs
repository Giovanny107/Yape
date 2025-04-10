namespace Transaction.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Entities.Transaction> AddAsync(Entities.Transaction entity);
        Task UpdateAsync(Entities.Transaction createdTransaction);
        Task<Entities.Transaction?> GetTransactionAsync(Guid transactionId);
    }
}
