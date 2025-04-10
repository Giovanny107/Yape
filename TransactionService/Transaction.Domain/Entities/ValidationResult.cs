namespace Transaction.Domain.Entities
{
    public class ValidationResult
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
