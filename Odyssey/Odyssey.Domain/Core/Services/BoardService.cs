using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Odyssey.Core.Constants;
using Odyssey.Domain.Core.Extensions;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence;

namespace Odyssey.Domain.Core.Services
{
    public class BoardService(IGameRegistry games,
        IDbContextFactory<ApplicationDbContext> dbContextFactory) : IBoardService
    {
        public Task<(Guid Id, BoardMetadata Board)> CreateBoard(BoardCreationOptions options)
        {
            var game = games.GetGame(options.GameId);
            return game.CreateBoard(options.OwnerId, options.Name);
        }

        public async Task<Result<BoardMetadata>> GetBoardAsync(string ownerId, Guid boardId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var board = await dbContext.BoardMetadatas
                .FirstOrDefaultAsync(q => q.OwnerId == ownerId && q.Id == boardId);
            return board.AsOptional().Map(BoardMetadata.FromDataModel).AsResult();
        }

        public async Task<List<(Guid Id, BoardMetadata Board)>> GetBoardsAsync(string ownerId, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.BoardMetadatas
                .Where(q => q.OwnerId == ownerId)
                .IfWhere(pagination.Last.HasValue, q => q.ModifiedOn < pagination.Last.Value!.ModifiedOn || (q.ModifiedOn == pagination.Last.Value!.ModifiedOn && q.Id > pagination.Last.Value!.Id))
                .OrderByDescending(q => q.ModifiedOn)
                .ThenBy(q => q.Id);

            var metadata = await query.Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize)).ToListAsync();
            return metadata.Select(m => (m.Id, BoardMetadata.FromDataModel(m))).ToList();
        }
        public async Task<List<(Guid Id, BoardMetadata Board)>> SearchBoardsAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetBoardsAsync(ownerId, pagination);

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.BoardMetadatas
                .Where(q => q.OwnerId == ownerId && q.SearchData.Contains(searchTerm))
                .IfWhere(pagination.Last.HasValue, q => q.ModifiedOn < pagination.Last.Value!.ModifiedOn || (q.ModifiedOn == pagination.Last.Value!.ModifiedOn && q.Id > pagination.Last.Value!.Id))
                .OrderByDescending(q => q.ModifiedOn)
                .ThenBy(q => q.Id)
                .Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize));

            var metadata = await query.ToListAsync();
            return metadata.Select(m => (m.Id, BoardMetadata.FromDataModel(m))).ToList();
        }
    }
}
