using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Odyssey.Core.Constants;
using Odyssey.Domain.Core.Extensions;
using Odyssey.Domain.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.Persistence;
using Odyssey.Persistence.Models;

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
            var model = meta.AsDataModel(new(ownerId, Guid.NewGuid()));

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FindAsync(ownerId)
                ?? throw new ArgumentException($"User {ownerId} not found.");
            user.BoardMetadatas.Add(model);
            await dbContext.SaveChangesAsync();

            return (model.EntityId, BoardMetadata.FromDataModel(model));
        }

        public async Task<Result<BoardMetadata>> GetBoardMetadataAsync(OwnedEntityId<Guid> id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var stringId = id.StringValue;
            var board = await dbContext.BoardMetadatas
                .FirstOrDefaultAsync(q => q.Id == stringId);
            return board.AsOptional().Map(BoardMetadata.FromDataModel).AsResult();
        }

        public async Task<List<(Guid Id, BoardMetadata Board)>> GetBoardMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.BoardMetadatas
                .Where(q => q.OwnerId == ownerId)
                .IfWhere(pagination.Last.HasValue, q => q.ModifiedOn < pagination.Last.Value!.ModifiedOn || (q.ModifiedOn == pagination.Last.Value!.ModifiedOn && q.EntityId > pagination.Last.Value!.Id))
                .OrderByDescending(q => q.ModifiedOn)
                .ThenBy(q => q.EntityId);

            var metadata = await query.Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize)).ToListAsync();
            return metadata.Select(m => (m.EntityId, BoardMetadata.FromDataModel(m))).ToList();
        }
        public async Task<List<(Guid Id, BoardMetadata Board)>> SearchBoardMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, AbsoluteDateTime ModifiedOn)> pagination = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetBoardMetadatasAsync(ownerId, pagination);

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.BoardMetadatas
                .Where(q => q.OwnerId == ownerId && q.SearchData.Contains(searchTerm))
                .IfWhere(pagination.Last.HasValue, q => q.ModifiedOn < pagination.Last.Value!.ModifiedOn || (q.ModifiedOn == pagination.Last.Value!.ModifiedOn && q.EntityId > pagination.Last.Value!.Id))
                .OrderByDescending(q => q.ModifiedOn)
                .ThenBy(q => q.EntityId)
                .Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize));

            var metadata = await query.ToListAsync();
            return metadata.Select(m => (m.EntityId, BoardMetadata.FromDataModel(m))).ToList();
        }

        public async Task<BoardMetadata> UpdateBoardMetadataAsync(OwnedEntityId<Guid> id, BoardMetadata board)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var model = dbContext.BoardMetadatas.Update(board.AsDataModel(id) with
            {
                ModifiedOn = clock.Now
            });

            await dbContext.SaveChangesAsync();
            return BoardMetadata.FromDataModel(model.Entity);
        }

        public async Task DeleteBoardMetadataAsync(OwnedEntityId<Guid> id)
        {
            var stringId = id.StringValue;
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.BoardMetadatas
                .Where(q => q.Id == stringId)
                .ExecuteDeleteAsync();
        }
    }
}
