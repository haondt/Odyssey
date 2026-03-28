using Newtonsoft.Json;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Domain.Core.Services
{
    public class PartyMemberIdJsonConverter : JsonConverter<PartyMemberId>
    {
        public override PartyMemberId ReadJson(JsonReader reader, Type objectType, PartyMemberId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String
                || reader.Value is not string valueString
                || valueString == null)
                throw new JsonSerializationException($"Cannot convert null value to nameof(PartyMemberId)");

            var sepIndex = valueString.IndexOf('+');
            var guid = Guid.Parse(valueString[..sepIndex]);
            var type = Enum.Parse<PartyMemberType>(valueString[(sepIndex + 1)..]);

            return new(guid, type);
        }

        public override void WriteJson(JsonWriter writer, PartyMemberId value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.Id}+{value.Type}");
        }
    }


}
