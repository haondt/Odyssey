using Haondt.Core.Models;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IBoardMetadataRepository
    {
        Task<List<(Guid Id, BoardMetadata Board)>> GetBoardMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default);
        Task<Result<BoardMetadata>> GetBoardMetadataAsync(OwnedEntityId<Guid> id);
        Task<List<(Guid Id, BoardMetadata Board)>> SearchBoardMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default);
        Task<(Guid Id, BoardMetadata Board)> CreateBoardMetadataAsync(string gameId, string ownerId, string name);
        Task<BoardMetadata> UpdateBoardMetadataAsync(OwnedEntityId<Guid> id, string name);
        Task DeleteBoardMetadataAsync(OwnedEntityId<Guid> id);
    }
}
