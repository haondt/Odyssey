using Microsoft.AspNetCore.Components;

namespace Haondt.Web.UI.Components.Containers
{
    public readonly record struct TabbedLayoutItem(
        string Title,
        IComponent Body,
        bool Active = false);

    public enum TabbedLayoutSpacing
    {
        SpaceApart,
        Stretch
    }
}
