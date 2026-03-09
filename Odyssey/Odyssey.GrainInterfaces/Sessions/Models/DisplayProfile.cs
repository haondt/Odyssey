namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class DisplayProfile : PartyMemberProfile
    {
        [Id(0)]
        public Guid Id { get; set; } = Guid.Empty;

        [Id(1)]
        public override string Name { get; set; } = "Display";
    }
}
