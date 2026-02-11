using Microsoft.AspNetCore.Components;
using Odyssey.UI.Core.Components;

namespace Odyssey.UI.Core.Services
{
    public interface IHasNavigationBarComponent : IComponent
    {
        public NavigationBar CreateNavigationBar();
    }
}
