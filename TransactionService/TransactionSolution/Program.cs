using Transaction.Application.Interfaces;
using Transaction.Application.Services;
using Transaction.Infrastructure.Services;
using Transaction.Domain.Interfaces;
using Transaction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Transaction.Infrastructure.Data;
using Confluent.Kafka;
using Transaction.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TransactionDb"));

builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "transaction-service-producer",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AllowAutoCreateTopics = true
};

builder.Services.AddSingleton<IKafkaConsumer>(sp =>
{
    var scope = sp.CreateScope();
    var transactionStatusService = scope.ServiceProvider.GetRequiredService<IValidationResultService>();    
    return new KafkaConsumer(consumerConfig, transactionStatusService);
});
builder.Services.AddScoped<IValidationResultService, ValidationResultService>();

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
