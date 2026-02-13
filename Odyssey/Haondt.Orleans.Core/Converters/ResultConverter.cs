using Haondt.Core.Models;
using Haondt.Orleans.Core.Surrogates;

namespace Haondt.Orleans.Core.Converters
{
    [RegisterConverter]
    public sealed class ResultConverter<T> : IConverter<Result<T>, ResultSurrogate<T>>
    {
        public Result<T> ConvertFromSurrogate(in ResultSurrogate<T> surrogate)
        {
            if (surrogate.IsSuccessful)
                return new(surrogate.Value!);
            return new();
        }

        public ResultSurrogate<T> ConvertToSurrogate(in Result<T> result)
        {
            if (result.IsSuccessful)
                return new() { IsSuccessful = true, Value = result.Value };
            return new() { IsSuccessful = false, Value = default };
        }
    }
}
