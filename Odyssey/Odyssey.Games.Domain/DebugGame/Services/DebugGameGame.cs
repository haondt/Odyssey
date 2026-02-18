using Microsoft.EntityFrameworkCore;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Games.Domain.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.Persistence;

namespace Odyssey.Games.Domain.DebugGame.Services
{
    public class DebugGameGame(
        ICachedDataService<DebugGameGameSettings> gameSettings,
        ICachedDataService<DebugGameBoard> boards,
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IClock clock) : IGame
    {
        public string Id => DebugGameConstants.GameId;


        public async Task<GameSettings> GetSettingsAsync(string hostUserId) => (await gameSettings.GetDataAsync(hostUserId)).Data;
        public async Task<(Guid Id, BoardMetadata Metadata)> CreateBoard(string ownerId, string name)
        {
            var board = new DebugGameBoard();
            var now = clock.Now;
            var meta = new BoardMetadata
            {
                Name = name,
                GameId = DebugGameConstants.GameId,
                CreatedOn = now,
                ModifiedOn = now
            };
            var model = meta.AsDataModel();

            using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {

                var user = await dbContext.Users.FindAsync(ownerId)
                    ?? throw new ArgumentException($"User {ownerId} not found.");
                user.BoardMetadatas.Add(model);
                await dbContext.SaveChangesAsync();
            }
            await boards.SetDataAsync(model.Id.ToString(), board, 0);

            return (model.Id, meta);
        }
    }
}
