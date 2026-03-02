using Odyssey.GrainInterfaces.Sessions;

namespace Odyssey.Grains.Sessions.Models
{
    [GenerateSerializer]
    public record PartyGrainState
    {
        [Id(0)]
        public required string JoinCode { get; set; }

        [Id(1)]
        public List<IPartyMember> Members { get; set; } = [];
    }
}
