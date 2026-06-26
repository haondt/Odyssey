using Microsoft.AspNetCore.Identity;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Services;
using Odyssey.Games.Domain.DebugGame.Models;
using Odyssey.GrainInterfaces.Core.Services;
using Odyssey.Persistence.Models;

namespace Odyssey.Services
{
    public class DeveloperDataSeeder(
        IClientGameRegistry gameRegistry,
        IHostSessionService sessionService,
        INameGenerator nameGenerator) : IDeveloperDataSeeder
    {
        public async Task SeedAsync()
        {
            var userId = await sessionService.GetUserIdAsync();
            var game = gameRegistry.GetGame(DebugGameConstants.GameId);
            var (boardId, board) = await game.Boards.CreateBoardAsync(userId, nameGenerator.Generate());
            var (sessionId, session) = await game.Sessions.CreateSessionAsync(userId, nameGenerator.Generate(), boardId, board.Name, false);
        }
    }
}
