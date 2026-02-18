using Haondt.Core.Extensions;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Client.Authentication.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Route(OdysseyRoutes.Host.Index)]
    public class HostController(IBoardService boards, ISessionService sessionService) : UIController
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
            // TODO
            var (boardId, board) = await boards.CreateBoard(new()
            {
                GameId = newBoard.Game,
                Name = newBoard.Name,
                OwnerId = (await sessionService.GetUserIdAsync()).Expect()
            });

            ResponseData.HxPushUrl($"{OdysseyRoutes.Host.Board.Index}/{boardId}");
            return await ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = board
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.New.Index)]
        public Task<IResult> GetNewBoard() => ComponentFactory.RenderComponentAsync<Components.NewBoardModal>();
    }
}
