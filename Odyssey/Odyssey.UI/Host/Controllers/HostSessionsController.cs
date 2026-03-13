using Haondt.Core.Extensions;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
    {
        [HttpGet(OdysseyRoutes.Host.Sessions.Index)]
        public Task<IResult> GetSessions() => ComponentFactory.RenderComponentAsync<Components.HostSessions>();

        [HttpGet(OdysseyRoutes.Host.Sessions.Search.Index)]
        public async Task<IResult> SearchSessions(
            [FromQuery] string? search,
            [FromQuery] TemporalContinuationData<Guid> last)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionList = string.IsNullOrWhiteSpace(search)
                ? await sessions.GetSessionMetadatasAsync(userId, last.PaginationOptionalTime)
                : await sessions.SearchSessionMetadatasAsync(userId, search, last.PaginationOptionalTime);

            return await ComponentFactory.RenderComponentAsync(new HostSessionsList
            {
                Sessions = sessionList,
                CurrentSearch = search.AsOptional(),
            });
        }

        [HttpGet(OdysseyRoutes.Host.Sessions.New.Index)]
        public Task<IResult> GetCreateSession() => ComponentFactory.RenderComponentAsync<Components.NewSessionModal>();

        [HttpPost(OdysseyRoutes.Host.Sessions.Index)]
        [ValidationState(typeof(NewSessionModalPanel), NewSessionModalPanel.Id)]
        public async Task<IResult> CreateSession([FromForm] NewSessionModel newSession)
        {
            var userId = await sessionService.GetUserIdAsync();
            var boardResult = await boards.GetBoardMetadataAsync((userId, newSession.Board));
            if (!boardResult.TryGetValue(out var board))
                return await RenderValidationComponentAsync(new() { [nameof(NewSessionModel.Board)] = "Board not found." });
            var game = gameRegistry.GetGame(board.GameId);
            var (sessionId, session) = await game.Sessions.CreateSessionAsync(userId, newSession.Name.Or(newSession.GeneratedName), newSession.Board, board.Name, newSession.Ephemeral);

            ResponseData
                .HxPushUrl(OdysseyRoutes.Host.Session.IdP(sessionId).IndexP);
            return await ComponentFactory.RenderComponentAsync(new EditSession
            {
                Id = sessionId
            });
        }

        [HttpGet(OdysseyRoutes.Host.Session.Id.Index)]
        public Task<IResult> GetSession(Guid id) => ComponentFactory.RenderComponentAsync(new EditSession { Id = id });

        [HttpPut(OdysseyRoutes.Host.Session.Id.Metadata.Index)]
        [ValidationState(typeof(EditSessionMetadataPanel))]
        public async Task<IResult> UpdateSessionMetadata(Guid id, [FromForm] EditSessionMetadataPanelModel update)
        {
            throw new ToastException("Not implemented yet");
        }

        [HttpGet(OdysseyRoutes.Host.Session.Id.Metadata.Index)]
        public async Task<IResult> GetEditSessionMetadata(Guid id)
        {
            return await ComponentFactory.RenderComponentAsync(new EditSessionMetadataPanel
            {
                Id = id
            });
        }

        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Index)]
        public Task<IResult> GetGameState(Guid id) => ComponentFactory.RenderComponentAsync(new EditSessionGameState { Id = id });

        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Raw.Index)]
        public Task<IResult> GetGameStateRaw(Guid id) => ComponentFactory.RenderComponentAsync(new EditSessionGameStateRaw { Id = id });
    }
}
