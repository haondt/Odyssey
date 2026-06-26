using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Containers
{
    public enum SectionColor
    {
        Secret
    }

    public static class SectionExtensions
    {
        extension(SectionColor color)
        {
            public Optional<string> CssClass => color switch
            {
                SectionColor.Secret => "section-color-secret",
                _ => new Optional<string>()
            };
        }
    }
}
