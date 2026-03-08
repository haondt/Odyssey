using Haondt.Web.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.UI.Core.Components;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EnrichUIRequestContextAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestContext = context.HttpContext.RequestServices.GetRequiredService<UIRequestContext>();
            var request = context.HttpContext.Request;
            var requestData = request.AsRequestData();
            if (requestData.Headers.TryGetValue<string>(BottomSheetContentContainer.CurrentRoleHeader).TryGetValue(out var currentUriHeader))
            {
                requestContext.BottomSheetRelayUri = currentUriHeader;
            }
        }
    }
}
