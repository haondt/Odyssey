using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Display.Models;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Display.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DisplaySessionAttribute : ActionFilterAttribute
    {
        public const string DisplayIdQueryParameter = "displayId";
        public const string DisplayIdHeader = "Ody-Display-Id";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var requestData = request.AsRequestData();
            if (TryGetDisplayId(requestData, out var displayId))
            {
                var sessionContext = context.HttpContext.RequestServices.GetRequiredService<DisplaySessionContext>();
                sessionContext.DisplayId = displayId;
                return;
            }

            var responseData = context.HttpContext.Response.AsResponseData();
            if (requestData.IsHxRequest())
                responseData.HxReswap("none");

            var url = QueryHelpers.AddQueryString(OdysseyRoutes.Display.Party.Index, DisplayIdQueryParameter, Guid.NewGuid().ToString());
            if (request.Method == HttpMethods.Get)
                context.Result = new RedirectResult(url);
            else
            {
                responseData.Status(400);
                responseData.HxRedirect(url);
                context.Result = new EmptyResult();
            }

        }

        public static bool TryGetDisplayId(IRequestData requestData, out Guid displayId)
        {
            if (TryGetDisplayIdFromQuery(requestData).TryGetValue(out displayId)
                || TryGetDisplayIdFromHeader(requestData).TryGetValue(out displayId))
                return true;
            return false;
        }

        private static Optional<Guid> TryGetDisplayIdFromQuery(IRequestData requestData) => (requestData.Query.TryGetValue<string>(DisplayIdQueryParameter)
            .TryGetValue(out var displayIdString) && Guid.TryParse(displayIdString, out var displayId))
            ? new(displayId) : new();
        private static Optional<Guid> TryGetDisplayIdFromHeader(IRequestData requestData) => (requestData.Headers.TryGetValue<string>(DisplayIdHeader)
            .TryGetValue(out var displayIdString) && Guid.TryParse(displayIdString, out var displayId))
            ? new(displayId) : new();

    }
}
