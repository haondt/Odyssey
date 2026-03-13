using Haondt.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Haondt.Web.UI.Models
{
    public class HxVals
    {
        private readonly Dictionary<string, object?> _inner = [];
        public object? this[string key]
        {
            get => _inner[key];
            set => _inner[key] = value;
        }

        public static JsonSerializerSettings SerializerSettings;
        static HxVals()
        {
            SerializerSettings = new();
            SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            SerializerSettings.Formatting = Formatting.None;
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.Converters.Add(new AbsoluteDateTimeJsonConverter());
            SerializerSettings.Converters.Add(new SimpleGenericOptionalJsonConverter());
        }

        public override string ToString() => Serialize(_inner);

        public static string Serialize(object obj)
        {
            var jo = JObject.FromObject(obj, JsonSerializer.Create(SerializerSettings));
            var flat = new Dictionary<string, object?>();
            FlattenJToken("", jo, flat);
            return JsonConvert.SerializeObject(flat, SerializerSettings);
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
                    // omit null values
                    if (v.Value != null)
                        flat[prefix] = v.Value;
                    break;
            }
        }
    }
}
