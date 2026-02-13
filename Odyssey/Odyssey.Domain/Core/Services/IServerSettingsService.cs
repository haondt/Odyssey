using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IServerSettingsService
    {
        ValueTask<(ServerSettings Settings, int Version)> GetServerSettingsAsync();
        Task<int> SetServerSettingsAsync(ServerSettings settings, int version);
    }
}