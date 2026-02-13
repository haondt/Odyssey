using Haondt.Core.Models;

namespace Odyssey.Domain.Core.Models
{
    [GenerateSerializer]
    public record ServerSettings
    {
        public const string Key = "ServerSettings";

        [Id(0)]
        public Optional<bool> OpenRegistration { get; set; } = default;
    }

}
