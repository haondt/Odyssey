using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Sessions.Models;

namespace Odyssey.GrainInterfaces.Sessions
{
    public interface IDeviceGrain : IGrain<Guid>, IGrainWithGuidKey, IPartyMemberGrain
    {
        public Task SetProfileAsync(DeviceProfile profile);
        Task<DeviceProfile> GetProfileAsync();
    }
}
