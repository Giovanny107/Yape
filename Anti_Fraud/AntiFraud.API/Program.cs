using AntiFraud.Application.Services;
using Confluent.Kafka;
using AntiFraud.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add memory cache
builder.Services.AddMemoryCache();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "transaction-service-producer",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true
};

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

// Register application services
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddScoped<ITransactionValidationService, TransactionValidationService>();
builder.Services.AddSingleton<IKafkaConsumer>(sp =>
{
    var scope = sp.CreateScope();
    var transactionStatusService = scope.ServiceProvider.GetRequiredService<ITransactionValidationService>();
    var kafkaProducer = sp.GetRequiredService<IKafkaProducer>();
    return new KafkaConsumer(consumerConfig, transactionStatusService, kafkaProducer);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Task.Run(() =>
{
    IKafkaConsumer consumer = app.Services.GetRequiredService<IKafkaConsumer>();
    consumer.StartConsuming();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); 