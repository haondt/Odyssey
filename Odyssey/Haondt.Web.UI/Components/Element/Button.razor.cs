using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum ButtonStyle
    {
        /// <summary>
        /// Solid fill
        /// </summary>
        Fill,
        /// <summary>
        /// No fill
        /// </summary>
        Ghost,
        /// <summary>
        /// No fill, no padding
        /// </summary>
        Skeleton,
        /// <summary>
        /// Add colored border
        /// </summary>
        Outline,
        /// <summary>
        /// Show hover and add border
        /// </summary>
        Border,
        /// <summary>
        /// Show hover
        /// </summary>
        Borderless
    }
    public enum ButtonColor
    {
        Text,
        TextWeak,
        Primary,
        Danger,
        Success
    }

    public enum ButtonType
    {
        Button,
        Submit,
        Reset,
        CloseModal
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
                ButtonStyle.Outline => "button-style-outline",
                ButtonStyle.Border => "button-style-border",
                ButtonStyle.Borderless => "button-style-borderless",
                _ => new Optional<string>()
            };
        }

        extension(ButtonColor color)
        {
            public Optional<string> CssClass => color switch
            {
                ButtonColor.Text => "button-color-text",
                ButtonColor.TextWeak => "button-color-text-weak",
                ButtonColor.Primary => "button-color-primary",
                ButtonColor.Danger => "button-color-danger",
                ButtonColor.Success => "button-color-success",
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
                ButtonType.CloseModal => "button",
                _ => "button"
            };
        }
    }

}
