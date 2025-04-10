using AntiFraud.Domain;

namespace AntiFraud.Application.Services;

public class TransactionValidationService : ITransactionValidationService
{    
    private readonly Dictionary<Guid, decimal> _dailyAccumulation = [];
    const decimal MAX_SINGLE_TRANSACTION = 2000m;
    const decimal MAX_DAILY_ACCUMULATED = 20000m;

    public string ValidateTransaction(Transaction transaction)
    {        
        if (transaction.Value > MAX_SINGLE_TRANSACTION)
        {
            return "Rejected";            
        }

        if (!_dailyAccumulation.ContainsKey(transaction.Id))
        {
            _dailyAccumulation[transaction.Id] = 0;
        }

        _dailyAccumulation[transaction.Id] += transaction.Value;

        if (_dailyAccumulation[transaction.Id] > MAX_DAILY_ACCUMULATED)
        {
            return "Rejected";
        }

        return "Approved";
    }    
} 