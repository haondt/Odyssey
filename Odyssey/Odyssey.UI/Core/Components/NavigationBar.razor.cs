using Haondt.Core.Models;
using Microsoft.AspNetCore.Components;
using Odyssey.Core.Exceptions;
using Odyssey.UI.Core.Models;

namespace Odyssey.UI.Core.Components
{
    public record NavigationBarDefault(string Role)
    {
        public List<NavigationBreadcrumb> Breadcrumbs { get; set; } = [];
        public string RoleRoute => Role switch
        {
            OdysseyRoles.Host => OdysseyRoutes.Host.Index,
            OdysseyRoles.Admin => OdysseyRoutes.Admin.Index,
            OdysseyRoles.Device => OdysseyRoutes.Device.Index,
            OdysseyRoles.Display => OdysseyRoutes.Display.Index,
            _ => OdysseyRoutes.Roles.Index
        };
        public string RoleText => Role switch
        {
            OdysseyRoles.Host => "Host",
            OdysseyRoles.Admin => "Admin",
            OdysseyRoles.Device => "Device",
            OdysseyRoles.Display => "Display",
            _ => throw ExceptionFactory.CasesExhaustedException(Role, "role")
        };
    }

    public record NavigationBarRoles();
    public record NavigationBarEmpty();

    public readonly record struct NavigationBreadcrumb(
        string Text,
        Optional<string> Href = default,
        bool Crushable = false);
    public record NavigationBarMinimal();

    public record NavigationBarAsyncFactory(Func<IServiceProvider, Task<IComponent>> Factory);
}
