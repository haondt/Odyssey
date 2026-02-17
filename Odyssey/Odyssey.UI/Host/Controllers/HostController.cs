using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;
using Odyssey.UI.Host.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Route(OdysseyRoutes.Host.Index)]
    public class HostController : UIController
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
        [ValidationErrors(typeof(NewBoardModalPanel), NewBoardModalPanel.Id)]
        public Task<IResult> CreateNewBoard([FromForm] NewBoardModel newBoard)
        {
            // TODO
            var newBoardId = Guid.NewGuid();
            ResponseData.HxPushUrl($"{OdysseyRoutes.Host.Board.Index}/{newBoardId}");
            return ComponentFactory.RenderComponentAsync(new EditBoard
            {
                Metadata = new() { Name = newBoard.Name }
            });
        }

        [HttpGet(OdysseyRoutes.Host.Boards.New.Index)]
        public Task<IResult> GetNewBoard() => ComponentFactory.RenderComponentAsync<Components.NewBoardModal>();
    }
}
