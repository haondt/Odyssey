using Haondt.Core.Models;

namespace Odyssey.GrainInterfaces.Sessions.Models
{
    [GenerateSerializer]
    public class HostPartyData
    {
        [Id(0)]
        public Dictionary<PartyMemberId, HostDisplayData> DisplayData { get; set; } = [];
        [Id(1)]
        public Dictionary<PartyMemberId, HostDeviceData> DeviceData { get; set; } = [];
    }
}
