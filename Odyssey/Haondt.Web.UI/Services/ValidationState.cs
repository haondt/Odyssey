using Haondt.Core.Models;

namespace Haondt.Web.UI.Services
{
    public class ValidationState : IValidationStateReader, IValidationStateWriter
    {
        public Dictionary<string, string> ValidationErrors { get; set; } = [];
        public Optional<string> ValidationSummary { get; set; }
        public bool IsValidation { get; set; }

        IReadOnlyDictionary<string, string> IValidationStateReader.ValidationErrors => ValidationErrors;
    }

    public interface IValidationStateReader
    {
        IReadOnlyDictionary<string, string> ValidationErrors { get; }
        Optional<string> ValidationSummary { get; }
        bool IsValidation { get; }
    }

    public interface IValidationStateWriter
    {
        Dictionary<string, string> ValidationErrors { get; set; }
        Optional<string> ValidationSummary { get; set; }
        bool IsValidation { get; set; }
    }
}
