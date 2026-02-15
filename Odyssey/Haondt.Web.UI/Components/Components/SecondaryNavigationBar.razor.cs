using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Components
{
    public readonly record struct SecondaryNavigationBarItem(
        string Text,
        Optional<string> Href = default,
        bool Active = false);

    public enum SecondaryNavigationBarSpacing
    {
        SpaceApart,
        Stretch
    }
}
