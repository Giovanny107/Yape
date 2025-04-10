using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace AntiFraud.Infrastructure.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                ClientId = "transaction-service-producer",
                AllowAutoCreateTopics = true
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _topic = "anti-fraud-responses";
        }

        public async Task SendMessage(dynamic response)
        {
            try
            {
                var message = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(response)
                };

                await _producer.ProduceAsync(_topic, message);
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Failed to deliver message: {e.Error.Reason}");
                throw;
            }
        }
    }
}
