namespace Transaction.Application.DTOs;

public class TransactionResponse
{
    public Guid TransactionExternalId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Value { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
} 