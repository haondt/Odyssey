namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class HostPartyData
    {
        [Id(0)]
        public Dictionary<Guid, HostDisplayData> DisplayData { get; set; } = [];
    }
}
