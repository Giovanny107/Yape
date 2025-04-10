using Transaction.Domain.Entities;

namespace Transaction.Application.Interfaces
{
    public interface IValidationResultService
    {
        Task ProcessValidationResult(ValidationResult validationResult);
    }
}
