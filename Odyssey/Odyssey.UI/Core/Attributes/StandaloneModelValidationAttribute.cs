using Odyssey.Client.Core.Exceptions;
using Odyssey.Client.Core.Services;
using Odyssey.UI.Core.Middlewares;
namespace Odyssey.UI.Core.Attributes
{
    /// <summary>
    /// Indicates that this method may throw <see cref="StandaloneModelBinderValidationException"/>s,
    /// which should be caught and applied by <see cref="StandaloneModelBinderValidationExceptionActionResultFactory"/>.
    /// </summary>
    /// <remarks>
    /// These exceptions can be thrown by <see cref="StandaloneModelBinder"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class StandaloneModelValidationAttribute : Attribute
    {
    }
}
