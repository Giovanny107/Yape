using Transaction.Domain.Enums;

namespace Transaction.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
} 