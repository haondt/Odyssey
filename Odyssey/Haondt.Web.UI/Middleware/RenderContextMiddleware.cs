using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Haondt.Web.UI.Middleware
{
    public class RenderContextMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var renderContext = context.RequestServices.GetRequiredService<IRenderContextMutator>();
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<ResetRenderContextAttribute>() != null)
                renderContext.IsReset = true;

            await next(context);
        }
    }
}
