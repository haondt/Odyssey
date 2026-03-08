using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EnforceBottomSheetContentAttribute : ActionFilterAttribute
    {
        public Type? Type { get; set; }
        public required string Uri { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var requestContext = context.HttpContext.RequestServices.GetRequiredService<UIRequestContext>();
            requestContext.BottomSheetTargetUri = Uri;
            if (Type != null)
                requestContext.BottomSheetTargetUriComponentType = Type;
        }
    }
}
