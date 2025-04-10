using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Transaction.Application.Interfaces;

namespace Transaction.Infrastructure.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                ClientId = "transaction-service-producer",
                AllowAutoCreateTopics = true
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _topic = configuration["Kafka:AntiFraudTopic"] ?? "anti-fraud-requests";
        }

        public async Task ProduceAsync(Domain.Entities.Transaction transaction)
        {
            try
            {
                var message = new Message<string, string>
                {
                    Key = transaction.Id.ToString(),
                    Value = JsonSerializer.Serialize(transaction)
                };

                await _producer.ProduceAsync(_topic, message);                
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Failed to deliver message: {e.Error.Reason}");
                throw;
            }
        }

        public void Dispose() => _producer.Dispose();
    }
}
