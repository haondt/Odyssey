using Odyssey.Domain.Core.Models;

namespace Odyssey.Domain.Core.Services
{
    public interface IBoardService
    {
        Task<(Guid Id, BoardMetadata Board)> CreateBoard(BoardCreationOptions options);
    }
}
