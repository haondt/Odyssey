using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Services
{
    public class GrainLeaseService(IGrainLeaseGrainFactory grainFactory) : IGrainLeaseService
    {
        public async Task<Result<GrainLease>> AcquireLeaseAsync(string key, TimeSpan ttl, CancellationToken cancellationToken = default)
        {
            var grain = grainFactory.GetGrain(key);
            var ownerId = Guid.NewGuid();
            var lease = await grain.AcquireAsync(ownerId, ttl, cancellationToken);
            return lease;
        }

        public async Task<GrainLease> WaitForLeaseAsync(string key, TimeSpan ttl, TimeSpan? maxWait = default, CancellationToken cancellationToken = default)
        {
            using var cts = maxWait.HasValue ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken) : null;
            cts?.CancelAfter(maxWait!.Value);
            cancellationToken = cts?.Token ?? cancellationToken;

            var grain = grainFactory.GetGrain(key);
            var ownerId = Guid.NewGuid();
            var lease = await grain.WaitForLeaseAsync(ownerId, ttl, cancellationToken);
            return lease;
        }
    }
}
