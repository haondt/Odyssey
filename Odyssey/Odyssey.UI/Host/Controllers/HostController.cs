using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Odyssey.Client.Authentication.Services;
using Odyssey.Client.Core.Services;
using Odyssey.Client.Host.Services;
using Odyssey.Domain.Core.Services;
using Odyssey.GrainInterfaces.Sessions.Models;
using Odyssey.UI.Core.Controllers;
using Odyssey.UI.Core.Exceptions;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;

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

        [HttpDelete(OdysseyRoutes.Host.Party.Members.Id.Index)]
        public async Task<IResult> RemovePartyMember(PartyMemberId id)
        {
            var party = await hostService.GetPartyAsync();
            await party.RemoveMemberAsync(id);
            return TypedResults.NoContent();
        }


        [HttpPut(OdysseyRoutes.Host.Party.Members.Id.Display.Index)]
        public async Task<IResult> UpdateDisplayPartyMember(PartyMemberId id, [FromForm] DisplayHostPartyMemberPanelModel model)
        {
            var party = await hostService.GetPartyAsync();
            var data = await party.GetDisplayDataAsync(id);
            await party.UpdateDisplayDataAsync(id, model.Apply(data));
            return TypedResults.NoContent();
        }
    }
}
