using Haondt.Core.Models;

namespace Haondt.Web.UI.Services
{
    public interface IValidationSummaryComponent : IValidationComponent
    {
        Optional<string> ValidationSummary { get; set; }
    }
}
