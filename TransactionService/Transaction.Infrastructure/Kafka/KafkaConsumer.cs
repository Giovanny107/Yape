using Confluent.Kafka;
using System.Text.Json;
using Transaction.Application.Interfaces;
using Transaction.Domain.Entities;

namespace Transaction.Infrastructure.Kafka
{
    public class KafkaConsumer(ConsumerConfig config, IValidationResultService validationResultService) : IKafkaConsumer
    {
        public void StartConsuming()
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("anti-fraud-responses");

            while (true)
            {
                try
                {
                    var cr = consumer.Consume();
                    var validationResult = JsonSerializer.Deserialize<ValidationResult>(cr.Message.Value);

                    if (validationResult != null)
                    {
                        validationResultService.ProcessValidationResult(validationResult);
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
