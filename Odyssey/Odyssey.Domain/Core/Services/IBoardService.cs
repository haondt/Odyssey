using Haondt.Core.Models;
using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IBoardService
    {
        Task<(Guid Id, BoardMetadata Board)> CreateBoard(BoardCreationOptions options);
        Task<List<(Guid Id, BoardMetadata Board)>> GetBoardsAsync(string ownerId, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default);
        Task<Result<BoardMetadata>> GetBoardAsync(string ownerId, Guid boardId);
        Task<List<(Guid Id, BoardMetadata Board)>> SearchBoardsAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default);
    }
}
