using Haondt.Core.Models;
using Haondt.Orleans.Core.Surrogates;

namespace Haondt.Orleans.Core.Converters
{
    [RegisterConverter]
    public sealed class OptionalConverter<T> : IConverter<Optional<T>, OptionalSurrogate<T>> where T : notnull
    {
        public Optional<T> ConvertFromSurrogate(in OptionalSurrogate<T> surrogate)
        {
            if (surrogate.HasValue)
                return new(surrogate.Value!);
            return new();
        }

        public OptionalSurrogate<T> ConvertToSurrogate(in Optional<T> value)
        {
            if (value.HasValue)
                return new() { HasValue = true, Value = value.Value };
            return new() { HasValue = false };
        }
    }
}
