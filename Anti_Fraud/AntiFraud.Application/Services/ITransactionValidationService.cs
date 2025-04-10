using AntiFraud.Domain;

namespace AntiFraud.Application.Services;

public interface ITransactionValidationService
{
    string ValidateTransaction(Transaction request);
} 