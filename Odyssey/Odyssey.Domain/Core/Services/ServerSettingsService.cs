using Odyssey.Domain.Core.Models;
using Odyssey.GrainInterfaces.Core;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.Domain.Core.Services
{
    public class ServerSettingsService(
        IDataStorageCacheGrainFactory<ServerSettings> cacheGrainFactory,
        IDataStorageGrainFactory<ServerSettings> grainFactory) : IServerSettingsService
    {
        private readonly IDataStorageCacheGrain<ServerSettings> _cacheGrain = cacheGrainFactory.GetGrain(ServerSettings.Key);
        private readonly IDataStorageGrain<ServerSettings> _grain = grainFactory.GetGrain(ServerSettings.Key);
        public ValueTask<(ServerSettings Settings, int Version)> GetServerSettingsAsync() => _cacheGrain.GetDataAsync();
        public Task<int> SetServerSettingsAsync(ServerSettings settings, int version) => _grain.SetDataAsync(settings, version);
    }
}
