using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;

namespace Odyssey.Client.Device.Services
{
    public interface IClientDeviceService
    {
        Task<Optional<MemberPartyDetails>> GetPartyAsync();
        Task<DetailedResult<MemberPartyDetails, JoinPartyReason>> JoinPartyAsync(string joinCode);
        Task<DetailedResult<LeavePartyReason>> LeavePartyAsync(string joinCode);
        Task ConfigureDeviceProfileAsync(DeviceProfile profile);
        Task<DeviceProfile> GetDeviceProfileAsync();
    }
}
