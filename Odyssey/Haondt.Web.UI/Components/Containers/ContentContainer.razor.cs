using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Containers
{
    public enum ContentContainerWidth
    {
        Default,
        Small,
        Large
    }

    /// <summary>
    /// Center and hero require the parent to be full height
    /// </summary>
    public enum ContentContainerLayout
    {
        Center,
        Hero,
        Top,
    }
    public static class ContentContainerExtensions
    {
        extension(ContentContainerLayout layout)
        {
            public Optional<string> CssClass => layout switch
            {
                ContentContainerLayout.Center => "content-container-layout-center",
                ContentContainerLayout.Hero => "content-container-layout-hero",
                ContentContainerLayout.Top => "content-container-layout-top",
                _ => new Optional<string>()
            };
        }

        extension(ContentContainerWidth width)
        {
            public Optional<string> CssClass => width switch
            {
                ContentContainerWidth.Default => "content-container-width-default",
                ContentContainerWidth.Small => "content-container-width-small",
                ContentContainerWidth.Large => "content-container-width-large",
                _ => new Optional<string>()
            };

        }
    }
}
