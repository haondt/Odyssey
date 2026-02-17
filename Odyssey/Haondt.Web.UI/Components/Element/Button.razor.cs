using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum ButtonStyle
    {
        Fill,
        /// <summary>
        /// No fill
        /// </summary>
        Ghost,
        /// <summary>
        /// No fill, no padding
        /// </summary>
        Skeleton
    }
    public enum ButtonColor
    {
        Text,
        Primary
    }

    public enum ButtonType
    {
        Button,
        Submit,
        Reset
    }

    public static class ButtonExtensions
    {
        extension(ButtonStyle type)
        {
            public Optional<string> CssClass => type switch
            {
                ButtonStyle.Fill => "button-style-fill",
                ButtonStyle.Ghost => "button-style-ghost",
                ButtonStyle.Skeleton => "button-style-skeleton",
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

        extension(ButtonType type)
        {
            public string TypeString => type switch
            {
                ButtonType.Button => "button",
                ButtonType.Submit => "submit",
                ButtonType.Reset => "reset",
                _ => "button"
            };
        }
    }

}
