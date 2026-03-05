using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Grains.Sessions.Models
{
    [GenerateSerializer]
    public class DisplayGrainState
    {
        [Id(0)]
        public Optional<IMemberPartyGrain> Party { get; set; }

        [Id(1)]
        public DisplayProfile Profile { get; set; } = new();

    }
}
