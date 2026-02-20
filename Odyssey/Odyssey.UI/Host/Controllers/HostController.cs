using Haondt.Core.Extensions;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Route(OdysseyRoutes.Host.Index)]
    public partial class HostController(IBoardService boards, ISessionService sessionService) : UIController
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
            var (boardId, board) = await boards.CreateBoard(new()
            {
                GameId = newBoard.Game,
                Name = newBoard.Name,
                OwnerId = await sessionService.GetUserIdAsync()
            });

            ResponseData.HxPushUrl($"{OdysseyRoutes.Host.Board.Index}/{boardId}");
            return await ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = board
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.Search.Index)]
        public async Task<IResult> SearchBoards(
            [FromQuery] string? search,
            [FromQuery] TemporalContinuationData<Guid> last)
        {
            var userId = await sessionService.GetUserIdAsync();
            var boardList = string.IsNullOrWhiteSpace(search)
                ? await boards.GetBoardsAsync(userId, last.Pagination)
                : await boards.SearchBoardsAsync(userId, search, last.Pagination);

            return await ComponentFactory.RenderComponentAsync(new HostBoardsList
            {
                Boards = boardList,
                CurrentSearch = search.AsOptional(),
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.New.Index)]
        public Task<IResult> GetNewBoard() => ComponentFactory.RenderComponentAsync<Components.NewBoardModal>();

        [HttpGet($"{OdysseyRoutes.Host.Board.Index}/{{id}}")]
        public async Task<IResult> GetBoard(Guid id)
        {
            throw new KeyNotFoundException("the key was not found");
            var result = await boards.GetBoardAsync(await sessionService.GetUserIdAsync(), id);
            if (!result.TryGetValue(out var metadata))
                throw new NotFoundErrorPageException();
            return await ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = metadata
            });
        }
    }
}
