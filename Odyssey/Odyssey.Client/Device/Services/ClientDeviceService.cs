using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.GrainInterfaces.Sessions;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.GrainInterfaces.Sessions.Reasons;

namespace Odyssey.Client.Device.Services
{
    public class ClientDeviceService(IDeviceSessionService sessionService, IGrainFactory<Guid, IDeviceGrain> grainFactory) : IClientDeviceService
    {
        public Task ConfigureDeviceProfileAsync(DeviceProfile profile)
        {
            var grain = grainFactory.GetGrain(sessionService.DeviceId);
            return grain.SetProfileAsync(profile);
        }

        public Task<DeviceProfile> GetDeviceProfileAsync()
        {
            var grain = grainFactory.GetGrain(sessionService.DeviceId);
            return grain.GetProfileAsync();
        }

        public Task<Optional<MemberPartyDetails>> GetPartyAsync()
        {
            var grain = grainFactory.GetGrain(sessionService.DeviceId);
            return grain.GetMemberPartyAsync();
        }

        public async Task<DetailedResult<MemberPartyDetails, JoinPartyReason>> JoinPartyAsync(string joinCode)
        {
            var grain = grainFactory.GetGrain(sessionService.DeviceId);
            var joinResult = await grain.JoinPartyAsync(joinCode);
            if (!joinResult.IsSuccessful)
                return new(joinResult.Reason);

            var partyDetails = await joinResult.Value.GetPartyDetailsAsync(new(sessionService.DeviceId, PartyMemberType.Device), await grain.GetMemberProfileAsync());
            return new(partyDetails);
        }

        public Task<DetailedResult<LeavePartyReason>> LeavePartyAsync(string joinCode)
        {
            var grain = grainFactory.GetGrain(sessionService.DeviceId);
            return grain.LeavePartyAsync(joinCode);
        }
    }
}
