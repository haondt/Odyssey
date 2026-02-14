using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Containers
{
    public enum ContentContainerWidth
    {
        Default,
        Small
    }
    public enum ContentContainerLayout
    {
        Center,
        Hero,
    }
    public static class ContentContainerExtensions
    {
        extension(ContentContainerLayout layout)
        {
            public Optional<string> CssClass => layout switch
            {
                ContentContainerLayout.Center => "content-container-layout-center",
                ContentContainerLayout.Hero => "content-container-layout-hero",
                _ => new Optional<string>()
            };
        }

        extension(ContentContainerWidth width)
        {
            public Optional<string> CssClass => width switch
            {
                ContentContainerWidth.Default => "content-container-width-default",
                ContentContainerWidth.Small => "content-container-width-small",
                _ => new Optional<string>()
            };

        }
    }
}
