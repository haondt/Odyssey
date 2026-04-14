using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Web.Components;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Components;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Components;
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
        private async Task<Result<IComponent>> GetHostSessionAsync()
        {
            var party = await hostService.GetPartyAsync();
            if (!(await party.GetCurrentSessionAsync()).TryGetValue(out var currentSession))
                return new();

            var userId = await sessionService.GetUserIdAsync();
            var sessionResult = await sessions.GetSessionMetadataAsync((userId, currentSession.SessionId));
            if (!sessionResult.TryGetValue(out var sessionMetadata))
            {
                await party.ClearCurrentSessionAsync(currentSession.SessionId);
                logger.LogWarning("Party {Party} for host {Host} had a current session that was missing metadata: {Session}", await party.GetJoinCodeAsync(), userId, currentSession.SessionId);
                return new();
            }

            return new HostSession
            {
                GameId = currentSession.GameId,
                SessionId = currentSession.SessionId,
                Status = currentSession.Status,
                Metadata = sessionMetadata
            };
        }

        [HttpGet(OdysseyRoutes.Host.Party.Session.Index)]
        public async Task<IResult> GetSession()
        {
            if (await GetHostSessionAsync() is { IsSuccessful: true, Value: var session })
                return await ComponentFactory.RenderComponentAsync(session);
            return TypedResults.Redirect(OdysseyRoutes.Host.Party.Index);
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
