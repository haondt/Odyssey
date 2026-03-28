using System.ComponentModel;
using System.Globalization;

namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    [Immutable]
    [TypeConverter(typeof(PartyMemberIdTypeConverter))]
    public readonly record struct PartyMemberId(Guid Id, PartyMemberType Type);
    public enum PartyMemberType
    {
        Display,
        Device
    }
    public class PartyMemberIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
            => destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            var s = (string)value;
            var sepIndex = s.IndexOf('+');
            var guid = Guid.Parse(s[..sepIndex]);
            var type = Enum.Parse<PartyMemberType>(s[(sepIndex + 1)..]);
            return new PartyMemberId(guid, type);
        }

        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            var id = (PartyMemberId)value!;
            return $"{id.Id}+{id.Type}";
        }
    }
}
