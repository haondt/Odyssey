namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class MemberPartyDetails
    {
        [Id(0)]
        public required string JoinCode { get; set; }

        [Id(1)]
        public required List<(PartyMemberId Id, PartyMemberProfile Profile)> Members { get; set; }
    }
}
