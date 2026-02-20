using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Odyssey.Core.Constants;
using Odyssey.Domain.Core.Extensions;
using Odyssey.Domain.Core.Models;
using Odyssey.Persistence;

namespace Odyssey.Domain.Core.Services
{
    public class BoardMetadataService(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IClock clock) : IBoardMetadataRepository
    {
        public async Task<(Guid Id, BoardMetadata Board)> CreateBoardMetadataAsync(string gameId, string ownerId, string name)
        {
            var now = clock.Now;
            var meta = new BoardMetadata
            {
                Name = name,
                GameId = gameId,
                CreatedOn = now,
                ModifiedOn = now
            };
            var model = meta.AsDataModel();

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FindAsync(ownerId)
                ?? throw new ArgumentException($"User {ownerId} not found.");
            user.BoardMetadatas.Add(model);
            await dbContext.SaveChangesAsync();

            return (model.Id, BoardMetadata.FromDataModel(model));
        }

        public async Task<Result<BoardMetadata>> GetBoardMetadataAsync(string ownerId, Guid boardId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var board = await dbContext.BoardMetadatas
                .FirstOrDefaultAsync(q => q.OwnerId == ownerId && q.Id == boardId);
            return board.AsOptional().Map(BoardMetadata.FromDataModel).AsResult();
        }

        public async Task<List<(Guid Id, BoardMetadata Board)>> GetBoardMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
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
        public async Task<List<(Guid Id, BoardMetadata Board)>> SearchBoardMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetBoardMetadatasAsync(ownerId, pagination);

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
