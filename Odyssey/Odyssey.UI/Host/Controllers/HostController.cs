using Haondt.Core.Extensions;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Models;
using Odyssey.Client.Core.Services;
using Odyssey.Domain.Core.Models;
using Odyssey.Domain.Core.Services;
using Odyssey.Persistence.Models;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Host.Index)]
    public partial class HostController(
        IClientGameRegistry gameRegistry,
        ISessionService sessionService,
        IBoardMetadataRepository boards) : UIController
    {
        [HttpGet]
        public IResult Get() => TypedResults.Redirect(OdysseyRoutes.Host.Party.Index);

        [HttpGet(OdysseyRoutes.Host.Party.Index)]
        public Task<IResult> GetParty() => ComponentFactory.RenderComponentAsync<Components.HostParty>();

        [HttpGet(OdysseyRoutes.Host.Sessions.Index)]
        public Task<IResult> GetSessions() => ComponentFactory.RenderComponentAsync<Components.HostSessions>();

        [HttpGet(OdysseyRoutes.Host.Boards.Index)]
        public Task<IResult> GetBoards() => ComponentFactory.RenderComponentAsync<Components.HostBoards>();

        [HttpPost(OdysseyRoutes.Host.Boards.Index)]
        [ValidationState(typeof(NewBoardModalPanel), NewBoardModalPanel.Id)]
        public async Task<IResult> CreateNewBoard([FromForm] NewBoardModel newBoard)
        {
            var game = gameRegistry.GetGame(newBoard.Game);
            var (boardId, board) = await game.Boards.CreateBoardAsync(await sessionService.GetUserIdAsync(), newBoard.Name);

            ResponseData.HxPushUrl($"{OdysseyRoutes.Host.Board.Index}/{boardId}");
            return await ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = board,
                Id = boardId
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.Search.Index)]
        public async Task<IResult> SearchBoards(
            [FromQuery] string? search,
            [FromQuery] TemporalContinuationData<Guid> last)
        {
            var userId = await sessionService.GetUserIdAsync();
            var boardList = string.IsNullOrWhiteSpace(search)
                ? await boards.GetBoardMetadatasAsync(userId, last.Pagination)
                : await boards.SearchBoardMetadatasAsync(userId, search, last.Pagination);

            return await ComponentFactory.RenderComponentAsync(new HostBoardsList
            {
                Boards = boardList,
                CurrentSearch = search.AsOptional(),
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.New.Index)]
        public Task<IResult> GetNewBoard() => ComponentFactory.RenderComponentAsync<Components.NewBoardModal>();

        [HttpGet($"{OdysseyRoutes.Host.Board.Index}/{{id}}")]
        public async Task<IResult> GetEditBoard(Guid id)
        {
            var result = await boards.GetBoardMetadataAsync(new(await sessionService.GetUserIdAsync(), id));
            if (!result.TryGetValue(out var metadata))
                throw new NotFoundErrorPageException();

            return await ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = metadata,
                Id = id
            });
        }

        [HttpPost(OdysseyRoutes.Host.Board.Id.Index)]
        [StandaloneModelValidation]
        public async Task<IResult> UpdateBoardState(Guid id)
        {
            var ownedId = new OwnedEntityId<Guid>(await sessionService.GetUserIdAsync(), id);
            var metadataResult = await boards.GetBoardMetadataAsync(ownedId);
            if (!metadataResult.TryGetValue(out var metadata))
                throw new NotFoundToastException($"Metadata for board {id} not found.");

            ClientGameHandle game = gameRegistry.GetGame(metadata.GameId);
            var boardUpdates = await game.UI.HandleBoardStateUpdateAsync(ownedId, HttpContext);

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
            if (boardUpdates.TryGetValue(out var update))
                layout.Components.Add(update);

            return await ComponentFactory.RenderComponentAsync(layout);
        }

        [HttpGet(OdysseyRoutes.Host.Board.Id.Reset.Index)]
        [ResetRenderContext]
        public async Task<IResult> ResetBoardState(Guid id)
        {
            var ownedId = new OwnedEntityId<Guid>(await sessionService.GetUserIdAsync(), id);
            var result = await boards.GetBoardMetadataAsync(ownedId);
            if (!result.TryGetValue(out var metadata))
                throw new NotFoundToastException($"Metadata for board {id} not found.");

            var game = gameRegistry.GetGame(metadata.GameId);
            var component = await game.UI.GetResetEditBoardComponentAsync(ownedId);

            return await ComponentFactory.RenderComponentAsync(component);
        }

        [HttpPost($"{OdysseyRoutes.Host.Board.Index}/{{id}}/metadata")]
        public async Task<IResult> UpdateBoardMetadata(Guid id, [FromForm] BoardMetadata metadata)
        {
            metadata = await boards.UpdateBoardMetadataAsync(new(await sessionService.GetUserIdAsync(), id), metadata);
            // TODO
            throw new NotImplementedException();
        }
    }
}
