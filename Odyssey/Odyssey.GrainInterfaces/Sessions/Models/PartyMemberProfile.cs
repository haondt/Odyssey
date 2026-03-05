namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public abstract class PartyMemberProfile
    {
        [Id(0)]
        public string Name { get; set; } = "";
        [Id(1)]
        public string Type { get; set; } = "Unknown";
    }
}
