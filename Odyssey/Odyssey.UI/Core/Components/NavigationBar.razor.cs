using Haondt.Core.Models;

namespace Odyssey.UI.Core.Components
{
    public record NavigationBarDefault(
        string Role,
        Optional<List<NavigationBreadcrumb>> Breadcrumbs = default);
    public record NavigationBarRoles();
    public record NavigationBarEmpty();

    public readonly record struct NavigationBreadcrumb(
        string Text,
        Optional<string> Path = default);
    public record NavigationBarMinimal();
}
