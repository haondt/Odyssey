using Haondt.Core.Extensions;
using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;

namespace Odyssey.UI.Core.Extensions
{
    public static class RequestDataExtensions
    {
        extension(IRequestData request)
        {
            public Optional<string> HxCurrentPath()
            {
                return request.Headers.TryGetValue<string>("Hx-Current-Url").Map(q => new Uri(q).AbsolutePath);
            }
            public bool IsHxHistoryRestoreRequest()
            {
                return request.Headers.TryGetValue<bool>("Hx-History-Restore-Request").Or(false); ;
            }
        }
    }
}
