namespace Transaction.Application.Interfaces
{
    public interface IKafkaProducerService : IDisposable
    {
        Task ProduceAsync(Domain.Entities.Transaction transaction);
    }
}
