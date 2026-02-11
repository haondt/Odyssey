using Haondt.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Attributes;
using Microsoft.AspNetCore.Components;
using Odyssey.UI.Core.Components;

namespace Odyssey.UI.Core.Services
{
    public class LayoutComponentFactory : ILayoutComponentFactory
    {
        public Task<IComponent> GetLayoutAsync(IComponent content)
        {
            var contentType = content.GetType();
            var layoutOptionsAttribute = content
                .GetType()
                .GetCustomAttributes(typeof(LayoutOptionsAttribute), false)
                .FirstOrDefault()
                .AsOptional()
                .Map(q => (LayoutOptionsAttribute)q);

            return Task.FromResult<IComponent>(new Layout
            {
                Content = content,
                LayoutOptionsAttribute = layoutOptionsAttribute
            });
        }
    }
}
