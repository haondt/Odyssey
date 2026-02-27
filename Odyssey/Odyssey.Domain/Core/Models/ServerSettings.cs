using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Models
{
    [GenerateSerializer]
    public record ServerSettings : IDataStorageData<ServerSettings>
    {
        public const string Key = "ServerSettings";

        [Id(0)]
        public Optional<bool> OpenRegistration { get; set; }

        public static ServerSettings Factory() => new();
    }

}
