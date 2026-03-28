namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class DisplayProfile : PartyMemberProfile
    {
        [Id(0)]
        public override string Name { get; set; } = "";
    }
}
