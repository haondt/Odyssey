using Haondt.Core.Models;

namespace Haondt.Orleans.Core.Converters
{
    [RegisterConverter]
    public sealed class AbsoluteDateTimeConverter : IConverter<AbsoluteDateTime, long>
    {
        public AbsoluteDateTime ConvertFromSurrogate(in long surrogate)
        {
            return AbsoluteDateTime.Create(surrogate);
        }

        public long ConvertToSurrogate(in AbsoluteDateTime value)
        {
            return value.UnixTimeSeconds;
        }
    }
}
