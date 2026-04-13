using Haondt.Core.Extensions;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Odyssey.Persistence.Models;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Components.Lobby;
using Odyssey.UI.Host.Components.Party;
using Odyssey.UI.Host.Components.Sessions;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
    {

        [HttpGet(OdysseyRoutes.Host.Sessions.New.Index)]
        public Task<IResult> GetCreateSession() => ComponentFactory.RenderComponentAsync<NewSessionModal>();

        [HttpPost(OdysseyRoutes.Host.Sessions.Index)]
        [ValidationState(typeof(FieldInvalidator))]
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
                Id = sessionId,
                Session = session
            });
        }

        [HttpDelete(OdysseyRoutes.Host.Session.Id.Index)]
        public async Task<IResult> DeleteSession(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionResult = await sessions.GetSessionMetadataAsync((userId, id));
            if (sessionResult.TryGetValue(out var session))
            {
                var game = gameRegistry.GetGame(session.GameId);
                await game.Sessions.DeleteSessionAsync((userId, id));
            }

            ResponseData
                .HxPushUrl(OdysseyRoutes.Host.Sessions.Index);
            return await ComponentFactory.RenderComponentAsync<HostSessions>();
        }

        [HttpPost(OdysseyRoutes.Host.Session.Id.GameState.Reset.Index)]
        public async Task<IResult> ResetSession(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await GetSessionMetadataOrErrorToastAsync(id);
            var game = gameRegistry.GetGame(session.GameId);
            await game.Sessions.ResetSessionAsync((userId, id));

            return await ComponentFactory.RenderComponentAsync(new EditSession
            {
                Id = id,
                Session = session
            });
        }


        [HttpGet(OdysseyRoutes.Host.Session.Id.Index)]
        public async Task<IResult> GetSession(Guid id) =>
            await ComponentFactory.RenderComponentAsync(new EditSession { Id = id, Session = await GetSessionMetadataOrErrorPage(id) });


        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Index)]
        public async Task<IResult> GetGameState(Guid id) =>
            await ComponentFactory.RenderComponentAsync(new EditSessionGameState { Id = id, Session = await GetSessionMetadataOrErrorPage(id) });

        [HttpPost(OdysseyRoutes.Host.Session.Id.GameState.Index)]
        [StandaloneModelValidation(ShowToast = true)]
        public async Task<IResult> UpdateGameState(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await GetSessionMetadataOrErrorToastAsync(id);
            var game = gameRegistry.GetGame(session.GameId);
            var result = await game.UI.HandleGameStateUpdateAsync((userId, id), HttpContext);

            var layout = new AppendComponentLayout
            {
                Components = [
                    new Toast
                    {
                        Severity = ToastSeverity.Success,
                        Text = "Board updated"
                    }
                ]
            };
            if (result.TryGetValue(out var update))
                layout.Components.Add(update);

            return await ComponentFactory.RenderComponentAsync(layout);
        }

        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Reset.Index)]
        [ResetRenderContext]
        public async Task<IResult> ResetGameState(Guid id) =>
            await ComponentFactory.RenderComponentAsync(new EditSessionGameState { Id = id, Session = await GetSessionMetadataOrErrorPage(id) });

        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Raw.Index)]
        public async Task<IResult> GetGameStateRaw(Guid id) =>
            await ComponentFactory.RenderComponentAsync(new EditSessionGameStateRaw { Id = id, Session = await GetSessionMetadataOrErrorPage(id) });

        [HttpGet(OdysseyRoutes.Host.Session.Id.GameState.Raw.Reset.Index)]
        [ResetRenderContext]
        public async Task<IResult> ResetGameStateRaw(Guid id) =>
            await ComponentFactory.RenderComponentAsync(new EditSessionGameStateRaw { Id = id, Session = await GetSessionMetadataOrErrorPage(id) });

        [HttpPost(OdysseyRoutes.Host.Session.Id.GameState.Raw.Index)]
        [ResetRenderContext]
        public async Task<IResult> UpdateGameStateRaw(Guid id, [FromForm] string state)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await GetSessionMetadataOrErrorToastAsync(id);
            var game = gameRegistry.GetGame(session.GameId);
            try
            {
                await game.Sessions.UpdateGameStateFromSerializedAsync((userId, id), state);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Failed to parse json while updating raw game state.");
                throw new ToastException("Failed to parse json. Please ensure it matches the expected format.")
                {
                    StatusCode = 400
                };
            }

            return await ComponentFactory.RenderComponentAsync(new AppendComponentLayout
            {
                Components = new()
                {
                    new Toast
                    {
                        Severity = ToastSeverity.Success,
                        Text = "Session updated"
                    },
                    // to reformat the json
                    new EditSessionGameStateRaw
                    {
                        Id = id,
                        Session = await GetSessionMetadataOrErrorPage(id),
                        // we are nesting inside an AppendComponentLayout which means the Reswap checks in the component factory wont kick in
                        // so we need to manually instigate the swap
                        HxSwapOob = true
                    }
                }
            });
        }

        [HttpPost(OdysseyRoutes.Host.Session.Id.Launch.Index)]
        public async Task<IResult> LaunchSession(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionMetadata = await sessions.UpdateSessionMetadataAsync((userId, id), minimumPhase: SessionPhase.InProgress);

            var party = await hostService.GetPartyAsync();
            await party.SetCurrentSessionAsync(sessionMetadata.GameId, id, GrainInterfaces.Sessions.SessionStatus.Lobby);

            ResponseData.HxPushUrl(OdysseyRoutes.Host.Party.Session.Index);
            return await ComponentFactory.RenderComponentAsync<HostSession>();
        }
    }
}
