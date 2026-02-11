using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum ButtonType
    {
        Fill,
        Ghost
    }
    public enum ButtonColor
    {
        Text,
        Primary
    }

    public static class ButtonExtensions
    {
        extension(ButtonType type)
        {
            public Optional<string> CssClass => type switch
            {
                ButtonType.Fill => "button-type-fill",
                ButtonType.Ghost => "button-type-ghost",
                _ => new Optional<string>()
            };
        }

        extension(ButtonColor color)
        {
            public Optional<string> CssClass => color switch
            {
                ButtonColor.Text => "button-color-text",
                ButtonColor.Primary => "button-color-primary",
                _ => new Optional<string>()
            };
        }
    }

}
