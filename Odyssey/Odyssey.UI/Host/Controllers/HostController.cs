using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Host.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Host.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(OdysseyRoutes.Host.Index)]
    public partial class HostController(
        IClientGameRegistry gameRegistry,
        IHostSessionService sessionService,
        IBoardMetadataRepository boards,
        ISessionMetadataRepository sessions,
        IClientHostService hostService,
        ILogger<HostController> logger,
        IOptions<UISettings> uiOptions) : UIController
    {
        [HttpGet]
        public IResult Get() => TypedResults.Redirect(OdysseyRoutes.Host.Party.Index);

        [HttpGet(OdysseyRoutes.Host.Party.Index)]
        public Task<IResult> GetParty() => ComponentFactory.RenderComponentAsync<Components.HostParty>();

        [HttpPost(OdysseyRoutes.Host.Party.Reset.Index)]
        public async Task<IResult> ResetParty()
        {
            var party = await hostService.GetPartyAsync();
            await party.ResetPartyAsync();
            return TypedResults.NoContent();
        }
    }
}
