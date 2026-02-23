using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IGrainLeaseService
    {
        Task<Result<GrainLease>> AcquireLeaseAsync(string key, TimeSpan ttl, CancellationToken cancellationToken = default);
        Task<GrainLease> WaitForLeaseAsync(string key, TimeSpan ttl, TimeSpan? maxWait = null, CancellationToken cancellationToken = default);
    }
}
