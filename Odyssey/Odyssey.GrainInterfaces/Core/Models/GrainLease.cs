namespace Odyssey.GrainInterfaces.Core.Models
{
    [GenerateSerializer]
    public sealed class GrainLease(IGrainLeaseGrain grain, Guid ownerId) : IAsyncDisposable
    {
        [Id(0)]
        private int _disposed;
        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
                await grain.ReleaseAsync(ownerId);
        }
    }
}
