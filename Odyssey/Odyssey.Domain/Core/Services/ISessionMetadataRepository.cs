using Haondt.Core.Models;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface ISessionMetadataRepository
    {
        Task<List<(Guid Id, SessionMetadata Session)>> GetSessionMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default);
        Task<Result<SessionMetadata>> GetSessionMetadataAsync(OwnedEntityGuid id);
        Task<List<(Guid Id, SessionMetadata Session)>> SearchSessionMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default);
        Task<SessionMetadata> UpdateSessionMetadataAsync(OwnedEntityGuid id, string name);
        Task DeleteSessionMetadataAsync(OwnedEntityGuid id);
        Task<(Guid Id, SessionMetadata Session)> CreateSessionMetadataAsync(string gameId, string ownerId, string name, Guid boardId, string boardName, bool ephemeral);
    }
}
