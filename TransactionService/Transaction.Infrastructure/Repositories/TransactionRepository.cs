using Microsoft.EntityFrameworkCore;
using Transaction.Domain.Interfaces;
using Transaction.Infrastructure.Data;

namespace Transaction.Infrastructure.Repositories
{
    public class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
    { 
        private readonly DbContext _context = context;

        public async Task<Domain.Entities.Transaction> AddAsync(Domain.Entities.Transaction entity)
        {
            await _context.Set<Domain.Entities.Transaction>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Domain.Entities.Transaction entity)
        {
            _context.Set<Domain.Entities.Transaction>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Domain.Entities.Transaction?> GetTransactionAsync(Guid transactionId)
        {
            return await _context.Set<Domain.Entities.Transaction>()
                .FirstOrDefaultAsync(t => t.Id == transactionId);
        }
    }
}
