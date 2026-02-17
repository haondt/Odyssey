using Haondt.Core.Extensions;
using Haondt.Core.Models;

namespace Haondt.Web.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationStateAttribute(Type componentType, string? hxSwapId = null) : Attribute
    {
        public Type ComponentType => componentType;
        public Optional<string> HxSwapId { get; } = hxSwapId.AsOptional();
    }
}
