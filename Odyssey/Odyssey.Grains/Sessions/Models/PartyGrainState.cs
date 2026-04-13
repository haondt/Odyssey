using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.Grains.Sessions.Models
{
    [GenerateSerializer]
    public record PartyGrainState
    {
        [Id(0)]
        public required string JoinCode { get; set; }

        [Id(1)]
        public List<(PartyMemberId Id, IPartyMemberGrain Member)> Members { get; set; } = [];

        [Id(2)]
        public HostPartyData HostData { get; set; } = new();

        [Id(3)]
        public Optional<(string GameId, Guid SessionId, SessionStatus Status)> CurrentSession { get; set; }
    }
}
