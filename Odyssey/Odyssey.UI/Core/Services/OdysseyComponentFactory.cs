using Haondt.Core.Extensions;
using Haondt.Web.Components;
using Haondt.Web.Core.Attributes;
using Haondt.Web.Core.Extensions;
using Haondt.Web.Core.Http;
using Haondt.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Odyssey.UI.Core.Components;

namespace Odyssey.UI.Core.Services
{
    public class OdysseyComponentFactory(
        IHttpContextAccessor httpContext,
        ILayoutComponentFactory layoutFactory) : ComponentFactory(httpContext, layoutFactory)
    {
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly ILayoutComponentFactory _layoutFactory = layoutFactory;

        protected override Microsoft.AspNetCore.Components.IComponent EmbedLayoutIntoPage(Microsoft.AspNetCore.Components.IComponent layout)
        {
            return new OdysseyPage { Content = layout };
        }

        public override async Task<IResult> RenderComponentAsync(Microsoft.AspNetCore.Components.IComponent component, Type componentType, IRequestData? requestData = null, IResponseData? responseData = null)
        {
            IRequestData request = requestData ?? _httpContext.HttpContext?.Request.AsRequestData() ?? throw new ArgumentNullException("requestData");
            IResponseData response = responseData ?? _httpContext.HttpContext?.Response.AsResponseData() ?? throw new ArgumentNullException("responseData");

            var renderPageAttribute = componentType.GetCustomAttributes(typeof(RenderPageAttribute), inherit: false).FirstOrDefault().AsOptional().Map(q => (RenderPageAttribute)q);
            if (renderPageAttribute.TryGetValue(out var attr))
            {
                if (request.IsHxRequest())
                {
                    component = await _layoutFactory.GetLayoutAsync(component);
                    componentType = component.GetType();
                    response
                        .HxRetarget($"#{Odyssey.UI.Core.Components.Layout.Id}")
                        .HxReselect("unset")
                        .HxReswap("morph:outerHTML");
                }
                else
                {
                    component = await _layoutFactory.GetLayoutAsync(component);
                    component = EmbedLayoutIntoPage(component);
                    componentType = component.GetType();
                }
            }

            var rootComponent = new RootComponent
            {
                Component = component,
                Request = request,
                Response = response,
                Type = componentType
            };

            return new RazorComponentResult<RootComponent>(rootComponent.ToDictionary());
        }
    }
}
