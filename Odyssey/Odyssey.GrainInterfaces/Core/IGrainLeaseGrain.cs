using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Models;
using Orleans.Concurrency;

namespace Odyssey.GrainInterfaces.Core
{
    public interface IGrainLeaseGrain : IGrain<string>, IGrainWithStringKey
    {
        [AlwaysInterleave]
        Task<bool> ReleaseAsync(Guid ownerId, CancellationToken ct = default);
        Task<Result<GrainLease>> AcquireAsync(Guid ownerId, TimeSpan ttl, CancellationToken ct = default);
        Task<GrainLease> WaitForLeaseAsync(Guid ownerId, TimeSpan ttl, CancellationToken ct = default);
    }
}
