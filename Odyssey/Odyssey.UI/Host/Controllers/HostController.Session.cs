using Haondt.Core.Extensions;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Components;
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
        [HttpGet(OdysseyRoutes.Host.Party.Session.Index)]
        public async Task<IResult> GetSession()
        {
            var party = await hostService.GetPartyAsync();
            if (!(await party.GetCurrentSessionAsync()).HasValue)
                return TypedResults.Redirect(OdysseyRoutes.Host.Party.Index);
            return await ComponentFactory.RenderComponentAsync<HostSession>();
        }

        [HttpDelete(OdysseyRoutes.Host.Party.Session.Index)]
        public async Task<IResult> EndSession()
        {
            var party = await hostService.GetPartyAsync();
            if ((await party.GetCurrentSessionAsync()).HasValue)
                await party.ClearCurrentSessionAsync();

            return await ComponentFactory.RenderComponentAsync(new NotificationDialog
            {
                Message = "Session ended."
            });
        }
    }
}
