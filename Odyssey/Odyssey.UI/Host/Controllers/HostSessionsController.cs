using Haondt.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.UI.Core.Models;
using Odyssey.UI.Host.Components;

namespace Odyssey.UI.Host.Controllers
{
    public partial class HostController
    {
        [HttpGet(OdysseyRoutes.Host.Sessions.Index)]
        public Task<IResult> GetSessions() => ComponentFactory.RenderComponentAsync<Components.HostSessions>();

        [HttpGet(OdysseyRoutes.Host.Sessions.Search.Index)]
        public async Task<IResult> SearchSessions(
            [FromQuery] string? search,
            [FromQuery] TemporalContinuationData<Guid> last)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionList = string.IsNullOrWhiteSpace(search)
                ? await sessions.GetSessionMetadatasAsync(userId, last.Pagination)
                : await sessions.SearchSessionMetadatasAsync(userId, search, last.Pagination);

            return await ComponentFactory.RenderComponentAsync(new HostSessionsList
            {
                Sessions = sessionList,
                CurrentSearch = search.AsOptional(),
            });
        }

        [HttpGet(OdysseyRoutes.Host.Sessions.New.Index)]
        public Task<IResult> GetCreateSession() => ComponentFactory.RenderComponentAsync<Components.NewSessionModal>();
    }
}
