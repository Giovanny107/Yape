using Transaction.Application.Interfaces;
using Transaction.Application.DTOs;
using Transaction.Domain.Interfaces;
using Transaction.Domain.Enums;

namespace Transaction.Application.Services;

public class TransactionService(ITransactionRepository repository, IKafkaProducerService kafkaProducerService) : ITransactionService
{    
    private readonly ITransactionRepository _repository = repository;
    private readonly IKafkaProducerService _kafkaProducer = kafkaProducerService;

    public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionRequest request)
    {
        var transaction = new Domain.Entities.Transaction
        {
            Value = request.Value,
            SourceAccountId = request.SourceAccountId,
            TargetAccountId = request.TargetAccountId,
            TransferTypeId = request.TransferTypeId,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdTransaction = await _repository.AddAsync(transaction);

        try
        {
            // Send to Anti-Fraud service via Kafka
            await _kafkaProducer.ProduceAsync(createdTransaction);            
        }
        catch (Exception ex)
        {
            throw;
        }

        return MapToResponse(createdTransaction);
    }

    private static TransactionResponse MapToResponse(Domain.Entities.Transaction transaction)
    {
        return new TransactionResponse
        {
            TransactionExternalId = transaction.Id,
            CreatedAt = transaction.CreatedAt,
            Value = transaction.Value,
            Status = transaction.Status.ToString(),
            SourceAccountId = transaction.SourceAccountId,
            TargetAccountId = transaction.TargetAccountId,
            TransferTypeId = transaction.TransferTypeId
        };
    }
} 