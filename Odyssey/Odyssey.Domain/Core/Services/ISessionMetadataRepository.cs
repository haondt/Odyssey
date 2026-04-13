using Haondt.Core.Models;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface ISessionMetadataRepository
    {
        Task<List<(Guid Id, SessionMetadata Session)>> GetSessionMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default);
        Task<Result<SessionMetadata>> GetSessionMetadataAsync(OwnedEntityGuid id);
        Task<List<(Guid Id, SessionMetadata Session)>> SearchSessionMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default);
        Task DeleteSessionMetadataAsync(OwnedEntityGuid id);
        Task<(Guid Id, SessionMetadata Session)> CreateSessionMetadataAsync(string gameId, string ownerId, string name, Guid boardId, string boardName, bool ephemeral);
        Task<SessionMetadata> UpdateSessionMetadataAsync(OwnedEntityGuid id, Optional<string> name = default, Optional<bool> archived = default, Optional<SessionPhase> phase = default, Optional<SessionPhase> minimumPhase = default);
    }
}
