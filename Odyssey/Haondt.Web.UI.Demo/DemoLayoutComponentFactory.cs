using Haondt.Web.Services;
using Microsoft.AspNetCore.Components;

namespace Haondt.Web.UI.Demo
{
    public class DemoLayoutComponentFactory : ILayoutComponentFactory
    {
        public Task<IComponent> GetLayoutAsync(IComponent content)
        {
            return Task.FromResult<IComponent>(new DemoLayout
            {
                Content = content
            });
        }
    }
}
