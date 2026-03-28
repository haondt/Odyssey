namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class HostPartyDetails
    {
        [Id(0)]
        public required string JoinCode { get; set; }

        [Id(1)]
        public required List<(PartyMemberId Id, PartyMemberProfile Profile)> Members { get; set; }

        [Id(2)]
        public required HostPartyData Data { get; set; }
    }
}
