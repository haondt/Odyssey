using Haondt.Core.Extensions;
using Haondt.Web.Core.Extensions;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odyssey.Domain.Core.Models;
using Odyssey.UI.Core.Exceptions;
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
                ? await sessions.GetSessionMetadatasAsync(userId, last.PaginationOptionalTime)
                : await sessions.SearchSessionMetadatasAsync(userId, search, last.PaginationOptionalTime);

            return await ComponentFactory.RenderComponentAsync(new HostSessionsList
            {
                Sessions = sessionList,
                CurrentSearch = search.AsOptional(),
            });
        }


        [HttpPost(OdysseyRoutes.Host.Session.Id.Archive.Index)]
        public async Task<IResult> ArchiveSession(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await sessions.UpdateSessionMetadataAsync((userId, id), archived: true);
            // swapping the whole thing because we need to update the archive -> unarchive button
            return await ComponentFactory.RenderComponentAsync(new EditSession
            {
                Id = id,
                Session = session,
            });
        }

        [HttpPost(OdysseyRoutes.Host.Session.Id.Unarchive.Index)]
        public async Task<IResult> UnarchiveSession(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await sessions.UpdateSessionMetadataAsync((userId, id), archived: false);
            // swapping the whole thing because we need to update the unarchive -> archive button
            return await ComponentFactory.RenderComponentAsync(new EditSession
            {
                Id = id,
                Session = session,
            });
        }

        private async Task<SessionMetadata> GetSessionMetadataOrErrorPage(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionResult = await sessions.GetSessionMetadataAsync((userId, id));
            if (!sessionResult.TryGetValue(out var session))
                throw new NotFoundErrorPageException();
            return session;
        }

        private async Task<SessionMetadata> GetSessionMetadataOrErrorToast(Guid id)
        {
            var userId = await sessionService.GetUserIdAsync();
            var sessionResult = await sessions.GetSessionMetadataAsync((userId, id));
            if (!sessionResult.TryGetValue(out var session))
                throw new NotFoundToastException($"Could not retrieve session {id}.");
            return session;
        }

        [HttpPut(OdysseyRoutes.Host.Session.Id.Metadata.Index)]
        [ValidationState(typeof(FieldInvalidator))]
        public async Task<IResult> UpdateSessionMetadata(Guid id, [FromForm] EditSessionMetadataPanelModel update)
        {
            var userId = await sessionService.GetUserIdAsync();
            var session = await sessions.UpdateSessionMetadataAsync((userId, id), name: update.Name);
            ResponseData.HxTriggerAfterSwap("closeModal");
            return await ComponentFactory.RenderComponentAsync(new EditSessionMetadataSection
            {
                Session = session,
                HxSwapOob = true
            });
        }

        [HttpGet(OdysseyRoutes.Host.Session.Id.Metadata.Index)]
        public async Task<IResult> GetEditSessionMetadata(Guid id)
        {
            return await ComponentFactory.RenderComponentAsync(new EditSessionMetadataPanel
            {
                Id = id
            });
        }
    }
}
