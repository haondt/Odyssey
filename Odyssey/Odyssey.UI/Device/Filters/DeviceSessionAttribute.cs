using Haondt.Core.Models;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.Client.Device.Models;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Device.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DeviceSessionAttribute : ActionFilterAttribute
    {
        public const string DeviceIdQueryParameter = "deviceId";
        public const string DeviceIdHeader = "Ody-Device-Id";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var requestData = request.AsRequestData();
            if (TryGetDeviceId(requestData, out var deviceId))
            {
                var sessionContext = context.HttpContext.RequestServices.GetRequiredService<DeviceSessionContext>();
                sessionContext.DeviceId = deviceId;
                return;
            }

            var responseData = context.HttpContext.Response.AsResponseData();
            if (requestData.IsHxRequest())
                responseData.HxReswap("none");

            var url = QueryHelpers.AddQueryString(OdysseyRoutes.Device.Party.Index, DeviceIdQueryParameter, Guid.NewGuid().ToString());
            if (request.Method == HttpMethods.Get)
                context.Result = new RedirectResult(url);
            else
            {
                responseData.Status(400);
                responseData.HxRedirect(url);
                context.Result = new EmptyResult();
            }

        }

        public static bool TryGetDeviceId(IRequestData requestData, out Guid deviceId) => TryGetDeviceIdFromQuery(requestData).TryGetValue(out deviceId)
                || TryGetDeviceIdFromHeader(requestData).TryGetValue(out deviceId);

        private static Optional<Guid> TryGetDeviceIdFromQuery(IRequestData requestData) => (requestData.Query.TryGetValue<string>(DeviceIdQueryParameter)
            .TryGetValue(out var deviceIdString) && Guid.TryParse(deviceIdString, out var deviceId))
            ? new(deviceId) : new();
        private static Optional<Guid> TryGetDeviceIdFromHeader(IRequestData requestData) => (requestData.Headers.TryGetValue<string>(DeviceIdHeader)
            .TryGetValue(out var deviceIdString) && Guid.TryParse(deviceIdString, out var deviceId))
            ? new(deviceId) : new();

    }
}
