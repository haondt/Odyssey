using Haondt.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Odyssey.Domain.Core.Models
{
    public readonly record struct PaginationOptions<T>(Optional<T> Last = default, Optional<int> PageSize = default) : IValidatableObject where T : notnull
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageSize.HasValue && PageSize.Value <= 0)
                yield return new ValidationResult("Page size must be greater than or equal to 1", [nameof(PageSize)]);
            if (PageSize.HasValue && PageSize.Value > 250)
                yield return new ValidationResult("Page size must be less than or equal to 250", [nameof(PageSize)]);
        }

    }
}
