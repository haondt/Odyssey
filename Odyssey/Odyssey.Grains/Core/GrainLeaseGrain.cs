using Haondt.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Grains.Core
{
    [GenerateSerializer]
    public record GrainLeaseState
    {
        [Id(0)]
        public Guid? OwnerId { get; set; }
        [Id(1)]
        public AbsoluteDateTime? Expiry { get; set; }
    }

    public class GrainLeaseGrain : Grain, IGrainLeaseGrain
    {
        private readonly record struct LeaseRequest(Guid OwnerId, TimeSpan Duration);
        private readonly record struct LeaseTenancy(Guid OwnerId, AbsoluteDateTime Expiry, IGrainTimer Timer);
        private Optional<LeaseTenancy> _tenancy;
        private readonly Queue<(LeaseRequest Request, TaskCompletionSource Tcs)> _waiters = new();
        private readonly IClock _clock;
        private readonly IPersistentState<GrainLeaseState> _state;
        private readonly SemaphoreSlim _releaseSemaphore = new(1, 1);


        // default grain lifetime is 2 hours
        private const int _maximumLeaseDurationSeconds = 600;

        public GrainLeaseGrain(IClock clock, [PersistentState(nameof(GrainLeaseState), GrainConstants.GrainStorage)] IPersistentState<GrainLeaseState> state)
        {
            _clock = clock;
            _state = state;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (_state.State.OwnerId is not { } ownerId || _state.State.Expiry is not { } expiry)
                return;

            var now = _clock.Now;
            if (now >= expiry)
            {
                await _state.ClearStateAsync(cancellationToken);
                return;
            }

            var remaining = expiry - now;
            await SetTenancy(new(ownerId, remaining), now);
        }

        private async Task<GrainLease> SetTenancy(LeaseRequest request, AbsoluteDateTime now)
        {
            var timer = this.RegisterGrainTimer(ReleaseAsync, request.OwnerId, new()
            {
                KeepAlive = true,
                DueTime = request.Duration,
                Interleave = true,
                Period = Timeout.InfiniteTimeSpan
            });
            _tenancy = new(new(request.OwnerId, now + request.Duration, timer));

            _state.State = new() { OwnerId = _tenancy.Value.OwnerId, Expiry = _tenancy.Value.Expiry };
            await _state.WriteStateAsync();
            return new(this.AsReference<IGrainLeaseGrain>(), request.OwnerId);
        }

        public async Task<Result<GrainLease>> AcquireAsync(Guid ownerId, TimeSpan ttl, CancellationToken ct = default)
        {
            if (ttl.TotalSeconds > _maximumLeaseDurationSeconds)
                throw new ArgumentException($"Lease duration must be less than or equal to {_maximumLeaseDurationSeconds}", nameof(ttl));

            var now = _clock.Now;
            if (_tenancy.TryGetValue(out var tenancy) && now < tenancy.Expiry)
                return new();

            return await SetTenancy(new(ownerId, ttl), now);
        }

        public async Task<GrainLease> WaitForLeaseAsync(Guid ownerId, TimeSpan ttl, CancellationToken ct = default)
        {
            if (ttl.TotalSeconds > _maximumLeaseDurationSeconds)
                throw new ArgumentException($"Lease duration must be less than or equal to {_maximumLeaseDurationSeconds}", nameof(ttl));

            var now = _clock.Now;
            if (_tenancy.TryGetValue(out var tenancy) && now < tenancy.Expiry)
            {
                var tcs = new TaskCompletionSource();
                _waiters.Enqueue(new(new(ownerId, ttl), tcs));
                await tcs.Task.WaitAsync(ct);
                // tenancy was set by the previous release
                return new(this.AsReference<IGrainLeaseGrain>(), ownerId);
            }

            return await SetTenancy(new(ownerId, ttl), now);
        }

        public async Task<bool> ReleaseAsync(Guid ownerId, CancellationToken ct = default)
        {
            await _releaseSemaphore.WaitAsync(ct);
            try
            {
                if (_tenancy.HasValue)
                {
                    if (ownerId != _tenancy.Value.OwnerId)
                        return false;

                    await _state.ClearStateAsync(ct);
                    _tenancy.Value.Timer.Dispose();
                    _tenancy = new();
                }

                while (_waiters.TryDequeue(out var waiter))
                {
                    if (waiter.Tcs.Task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
                        continue;
                    // set tenancy for next waiter
                    await SetTenancy(waiter.Request, _clock.Now);
                    if (waiter.Tcs.TrySetResult())
                        break;
                    await _state.ClearStateAsync(ct);
                    _tenancy.Value.Timer.Dispose();
                    _tenancy = new();
                }
                return true;
            }
            finally
            {
                _releaseSemaphore.Release();
            }
        }
    }
}
