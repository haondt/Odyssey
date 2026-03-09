using Haondt.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Orleans.Serialization;

namespace Odyssey.Domain.Core.Models
{
    public class JsonUtils
    {

        public static void ConfigureOrleansSerializerOptions(OrleansJsonSerializerOptions options)
        {
            options.JsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            options.JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            options.JsonSerializerSettings.Formatting = Formatting.None;
            options.JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.JsonSerializerSettings.Converters.Add(new AbsoluteDateTimeJsonConverter());
            options.JsonSerializerSettings.Converters.Add(new GenericOptionalJsonConverter());
            options.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false
                }
            };
        }
    }
}
