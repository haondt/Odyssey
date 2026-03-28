namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class DeviceProfile : PartyMemberProfile
    {
        [Id(0)]
        public override string Name { get; set; } = "";
    }
}
