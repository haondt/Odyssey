using Haondt.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Odyssey.Client.Core.Exceptions
{
    public class StandaloneModelBinderValidationException(Type componentType, ModelStateDictionary model, Optional<string> hxSwapId = default) : Exception
    {
        public Type ComponentType => componentType;
        public ModelStateDictionary Model => model;
        public Optional<string> HxSwapId { get; } = hxSwapId;
    }
}
