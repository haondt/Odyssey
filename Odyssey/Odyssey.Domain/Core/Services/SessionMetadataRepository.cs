using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Odyssey.Core.Constants;
using Odyssey.Core.Models;
using Odyssey.Domain.Core.Extensions;
using Odyssey.Domain.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.Persistence;
using Odyssey.Persistence.Models;

namespace Odyssey.Domain.Core.Services
{
    public class SessionMetadataRepository(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IClock clock) : ISessionMetadataRepository
    {
        public async Task<(Guid Id, SessionMetadata Session)> CreateSessionMetadataAsync(
            string gameId, string ownerId, string name,
            Guid boardId, string boardName, bool ephemeral)
        {
            var now = clock.Now;
            var meta = new SessionMetadata
            {
                Name = name,
                GameId = gameId,
                CreatedOn = now,
                BoardId = boardId,
                BoardName = boardName,
                Archived = false,
                Ephemeral = ephemeral
            };
            var model = meta.AsDataModel((ownerId, Guid.NewGuid()));

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var user = await dbContext.Users.FindAsync(ownerId)
                ?? throw new ArgumentException($"User {ownerId} not found.");
            user.SessionMetadatas.Add(model);
            await dbContext.SaveChangesAsync();

            return (model.EntityId, SessionMetadata.FromDataModel(model));
        }

        public async Task<Result<SessionMetadata>> GetSessionMetadataAsync(OwnedEntityGuid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var board = await dbContext.SessionMetadatas
                .FirstOrDefaultAsync(q => q.Id == id);
            return board.AsOptional().Map(SessionMetadata.FromDataModel).AsResult();
        }

        public async Task<List<(Guid Id, SessionMetadata Session)>> GetSessionMetadatasAsync(string ownerId, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.SessionMetadatas
                .Where(q => q.OwnerId == ownerId && !q.Archived)
                .IfWhere(pagination.Last.HasValue && !pagination.Last.Value.LastPlayedOn.HasValue, q => q.EntityId > pagination.Last.Value.Id)
                .IfWhere(pagination.Last.HasValue && pagination.Last.Value.LastPlayedOn.HasValue, q => q.LastPlayedOn.HasValue && (q.LastPlayedOn.Value > pagination.Last.Value!.LastPlayedOn.Value! || (q.LastPlayedOn.Value == pagination.Last.Value!.LastPlayedOn.Value! && q.EntityId > pagination.Last.Value!.Id)))
                .OrderByDescending(q => q.LastPlayedOn == null)
                .ThenByDescending(q => q.LastPlayedOn)
                .ThenBy(q => q.EntityId);

            var metadata = await query.Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize)).ToListAsync();
            return metadata.Select(m => (m.EntityId, SessionMetadata.FromDataModel(m))).ToList();
        }
        public async Task<List<(Guid Id, SessionMetadata Session)>> SearchSessionMetadatasAsync(string ownerId, NormalizedString searchTerm, PaginationOptions<(Guid Id, Optional<AbsoluteDateTime> LastPlayedOn)> pagination = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetSessionMetadatasAsync(ownerId, pagination);

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var query = dbContext.SessionMetadatas
                .Where(q => q.OwnerId == ownerId && !q.Archived && q.SearchData.Contains(searchTerm))
                .IfWhere(pagination.Last.HasValue && !pagination.Last.Value.LastPlayedOn.HasValue, q => q.EntityId > pagination.Last.Value.Id)
                .IfWhere(pagination.Last.HasValue && pagination.Last.Value.LastPlayedOn.HasValue, q => q.LastPlayedOn.HasValue && (q.LastPlayedOn.Value > pagination.Last.Value!.LastPlayedOn.Value! || (q.LastPlayedOn.Value == pagination.Last.Value!.LastPlayedOn.Value! && q.EntityId > pagination.Last.Value!.Id)))
                .OrderByDescending(q => q.LastPlayedOn == null)
                .ThenByDescending(q => q.LastPlayedOn)
                .ThenBy(q => q.EntityId)
                .Take(pagination.PageSize.Or(OdysseyConstants.DefaultPageSize));

            var metadata = await query.ToListAsync();
            return metadata.Select(m => (m.EntityId, SessionMetadata.FromDataModel(m))).ToList();
        }

        public async Task<SessionMetadata> UpdateSessionMetadataAsync(OwnedEntityGuid id, Optional<string> name = default, Optional<bool> archived = default, Optional<SessionPhase> phase = default, Optional<SessionPhase> minimumPhase = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var session = await dbContext.SessionMetadatas
                .FirstOrDefaultAsync(q => q.Id == id)
                ?? throw new KeyNotFoundException($"Session with id {id} does not exist.");
            if (name.HasValue)
            {
                session.Name = name.Value;
                session.SearchData = SessionMetadataDataModel.CreateSearchData(name.Value, session.BoardName);
            }
            if (archived.HasValue)
                session.Archived = archived.Value;
            if (phase.HasValue)
                session.Status = phase.Value;
            else if (minimumPhase.HasValue && minimumPhase.Value > session.Status)
                session.Status = minimumPhase.Value;
            await dbContext.SaveChangesAsync();
            return SessionMetadata.FromDataModel(session);
        }

        public async Task DeleteSessionMetadataAsync(OwnedEntityGuid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.SessionMetadatas
                .Where(q => q.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
