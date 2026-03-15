using Haondt.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Haondt.Web.UI.Utils
{
    internal class JsonUtils
    {
        public static readonly JsonSerializerSettings SerializerSettings;
        static JsonUtils()
        {
            SerializerSettings = new();
            SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            SerializerSettings.Formatting = Formatting.Indented;
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.Converters.Add(new AbsoluteDateTimeJsonConverter());
            SerializerSettings.Converters.Add(new SimpleGenericOptionalJsonConverter());
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            };
        }

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, SerializerSettings);
        }

        public static T DeserializeObject<T>(string s)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<T>(s, SerializerSettings);
#pragma warning restore CS8603 // Possible null reference return.
        }

    }
}
