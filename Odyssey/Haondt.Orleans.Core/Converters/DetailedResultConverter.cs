using Haondt.Core.Models;
using Haondt.Orleans.Core.Surrogates;

namespace Haondt.Orleans.Core.Converters
{
    [RegisterConverter]
    public sealed class DetailedResultConverter<TReason> : IConverter<DetailedResult<TReason>, DetailedResultSurrogate<TReason>>
    {
        public DetailedResult<TReason> ConvertFromSurrogate(in DetailedResultSurrogate<TReason> surrogate) => surrogate.IsSuccessful ? new() : new(surrogate.Reason!);
        public DetailedResultSurrogate<TReason> ConvertToSurrogate(in DetailedResult<TReason> value) => new()
        {
            IsSuccessful = value.IsSuccessful,
            Reason = value.IsSuccessful ? default : value.Reason
        };
    }

    [RegisterConverter]
    public sealed class DetailedResultConverter<T, TReason> : IConverter<DetailedResult<T, TReason>, DetailedResultSurrogate<T, TReason>>
    {
        public DetailedResult<T, TReason> ConvertFromSurrogate(in DetailedResultSurrogate<T, TReason> surrogate) => surrogate.IsSuccessful ? new(surrogate.Value!) : new(surrogate.Reason!);
        public DetailedResultSurrogate<T, TReason> ConvertToSurrogate(in DetailedResult<T, TReason> value) => new()
        {
            IsSuccessful = value.IsSuccessful,
            Reason = value.IsSuccessful ? default : value.Reason,
            Value = value.IsSuccessful ? value.Value : default
        };
    }
}

