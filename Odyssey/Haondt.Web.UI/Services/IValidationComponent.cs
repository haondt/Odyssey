using Microsoft.AspNetCore.Components;

namespace Haondt.Web.UI.Services
{
    public interface IValidationComponent : IComponent
    {
        bool IsValidation { set; }
    }
}
