using AntiFraud.Application.Services;
using AntiFraud.Domain;
using Confluent.Kafka;
using System.Text.Json;

namespace AntiFraud.Infrastructure.Kafka
{
    public class KafkaConsumer(ConsumerConfig config, ITransactionValidationService transactionValidationService, IKafkaProducer kafkaProducer) : IKafkaConsumer
    {
        private readonly ConsumerConfig _config = config;
        private readonly ITransactionValidationService _transactionValidationService = transactionValidationService;
        private readonly IKafkaProducer _kafkaProducer = kafkaProducer;

        public void StartConsuming()
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe("anti-fraud-requests");

            while (true)
            {
                try
                {
                    var cr = consumer.Consume();
                    var statusUpdate = JsonSerializer.Deserialize<Transaction>(cr.Message.Value);

                    if (statusUpdate != null)
                    {
                        var result = _transactionValidationService.ValidateTransaction(statusUpdate);
                        var resultMessage = new
                        {
                            statusUpdate.Id,
                            Status = result
                        };

                        _kafkaProducer.SendMessage(resultMessage);

                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }
        }
    }
}
