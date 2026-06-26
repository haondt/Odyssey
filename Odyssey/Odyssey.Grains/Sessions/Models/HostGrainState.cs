using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Grains.Sessions.Models
{
    [GenerateSerializer]
    public record HostGrainState
    {
        [Id(0)]
        public HostSettings Settings { get; set; } = new();
    }
}
