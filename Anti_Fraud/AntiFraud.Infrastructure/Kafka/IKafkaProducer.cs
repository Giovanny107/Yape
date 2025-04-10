namespace AntiFraud.Infrastructure.Kafka
{
    public interface IKafkaProducer
    {
        Task SendMessage(dynamic response);
    }
}
