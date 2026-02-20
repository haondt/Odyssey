using Haondt.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Orleans.Serialization;

namespace Odyssey.Domain.Core.Models
{
    public class JsonUtils
    {
        public static JsonSerializerSettings HxValsSerializerSettings { get; }
        public static string SerializeHxVals(object obj)
        {
            var jo = JObject.FromObject(obj, JsonSerializer.Create(HxValsSerializerSettings));
            var flat = new Dictionary<string, object?>();
            FlattenJToken("", jo, flat);
            return JsonConvert.SerializeObject(flat, HxValsSerializerSettings);
        }

        private static void FlattenJToken(string prefix, JToken token, Dictionary<string, object?> flat)
        {
            switch (token)
            {
                case JObject o:
                    foreach (var prop in o.Properties())
                    {

                        FlattenJToken(string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}", prop.Value, flat);
                    }
                    break;
                case JArray a:
                    for (int i = 0; i < a.Count; i++)
                        FlattenJToken($"{prefix}[{i}]", a[i], flat);
                    break;
                case JValue v:
                    flat[prefix] = v.Value;
                    break;
            }
        }

        public static void ConfigureOrleansSerializerOptions(OrleansJsonSerializerOptions options)
        {
            options.JsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            options.JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            options.JsonSerializerSettings.Formatting = Formatting.None;
            options.JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            };
        }

        static JsonUtils()
        {
            HxValsSerializerSettings = new();
            HxValsSerializerSettings.TypeNameHandling = TypeNameHandling.None;
            HxValsSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            HxValsSerializerSettings.Formatting = Formatting.None;
            HxValsSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            HxValsSerializerSettings.Converters.Add(new AbsoluteDateTimeJsonConverter());
        }
    }
}
