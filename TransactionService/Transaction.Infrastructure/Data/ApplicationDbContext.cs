using Microsoft.EntityFrameworkCore;

namespace Transaction.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
