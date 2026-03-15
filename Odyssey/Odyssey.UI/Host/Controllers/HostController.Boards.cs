using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odyssey.Client.Core.Models;
using Odyssey.Core.Models;
using Odyssey.UI.Core.Attributes;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;
using Orleans.Storage;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
    {
        [HttpGet(OdysseyRoutes.Host.Boards.Index)]
        public Task<IResult> GetBoards() => ComponentFactory.RenderComponentAsync<Components.HostBoards>();

        [HttpPost(OdysseyRoutes.Host.Boards.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> CreateNewBoard([FromForm] NewBoardModel newBoard)
        {
            var game = gameRegistry.GetGame(newBoard.Game);
            var (boardId, board) = await game.Boards.CreateBoardAsync(await sessionService.GetUserIdAsync(), newBoard.Name);

            ResponseData
                .HxPushUrl($"{OdysseyRoutes.Host.Board.Index}/{boardId}");
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

        [HttpGet(OdysseyRoutes.Host.Boards.Suggest.Index)]
        public async Task<IResult> SuggestBoards([FromQuery] string? search)
        {
            var userId = await sessionService.GetUserIdAsync();
            var boardList = string.IsNullOrWhiteSpace(search)
                ? await boards.GetBoardMetadatasAsync(userId, new(PageSize: uiOptions.Value.SuggestionsPageSize))
                : await boards.SearchBoardMetadatasAsync(userId, search, new(PageSize: uiOptions.Value.SuggestionsPageSize));

            return await ComponentFactory.RenderComponentAsync(new FieldSuggestions
            {
                Values = boardList.Select(q => (Suggestion)(q.Board.Name, q.Id.ToString())).ToList()
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
        [StandaloneModelValidation(ShowToast = true)]
        public async Task<IResult> UpdateBoardState(Guid id)
        {
            var ownedId = new OwnedEntityGuid(await sessionService.GetUserIdAsync(), id);
            var metadataResult = await boards.GetBoardMetadataAsync(ownedId);
            if (!metadataResult.TryGetValue(out var metadata))
                throw new NotFoundToastException($"Metadata for board {id} not found.");

            ClientGameHandle game = gameRegistry.GetGame(metadata.GameId);
            Optional<IComponent> boardUpdates;
            try
            {
                boardUpdates = await game.UI.HandleBoardStateUpdateAsync(ownedId, HttpContext);
            }
            catch (InconsistentStateException ex)
            {
                logger.LogError(ex, $"Caught {nameof(InconsistentStateException)} while updating board state");
                throw new ToastException("Board was updated from another device. Reload the page to get the latest version.", ex)
                {
                    Severity = ToastSeverity.Error,
                    StatusCode = 409
                };
            }

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

        [HttpDelete(OdysseyRoutes.Host.Board.Id.Index)]
        public async Task<IResult> DeleteBoard(Guid id)
        {
            var ownedId = new OwnedEntityGuid(await sessionService.GetUserIdAsync(), id);
            var metadataResult = await boards.GetBoardMetadataAsync(ownedId);
            if (metadataResult.TryGetValue(out var metadata))
            {
                var game = gameRegistry.GetGame(metadata.GameId);
                await game.Boards.DeleteBoardAsync(ownedId);
            }
            ResponseData
                .HxPushUrl(OdysseyRoutes.Host.Boards.Index);
            return await ComponentFactory.RenderComponentAsync<Components.HostBoards>();
        }

        [HttpGet(OdysseyRoutes.Host.Board.Id.Reset.Index)]
        [ResetRenderContext]
        public async Task<IResult> ResetBoardState(Guid id)
        {
            var ownedId = new OwnedEntityGuid(await sessionService.GetUserIdAsync(), id);
            var result = await boards.GetBoardMetadataAsync(ownedId);
            if (!result.TryGetValue(out var metadata))
                throw new NotFoundToastException($"Metadata for board {id} not found.");

            var game = gameRegistry.GetGame(metadata.GameId);
            var component = await game.UI.GetResetEditBoardComponentAsync(ownedId);

            return await ComponentFactory.RenderComponentAsync(component);
        }

        [HttpPut(OdysseyRoutes.Host.Board.Id.Metadata.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> UpdateBoardMetadata(Guid id, [FromForm] EditBoardMetadataPanelModel update)
        {
            var ownedId = new OwnedEntityGuid(await sessionService.GetUserIdAsync(), id);
            var updated = await boards.UpdateBoardMetadataAsync(ownedId, update.Name);
            var game = gameRegistry.GetGame(updated.GameId);

            ResponseData.HxTriggerAfterSwap("closeModal");
            return await ComponentFactory.RenderComponentAsync(new EditBoardMetadataSection
            {
                Name = update.Name,
                GameName = await game.Settings.GetDisplayNameAsync(ownedId.OwnerId),
                HxSwapOob = true
            });

        }

        [HttpGet(OdysseyRoutes.Host.Board.Id.Metadata.Index)]
        public async Task<IResult> GetEditBoardMetadata(Guid id)
        {
            return await ComponentFactory.RenderComponentAsync(new EditBoardMetadataPanel
            {
                BoardId = id
            });
        }
    }
}
