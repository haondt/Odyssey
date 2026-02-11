using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum FrameDirection
    {
        Col,
        Row
    }

    public enum FrameJustification
    {
        None,
        Start,
        End,
        Center,
        SpaceBetween,
        SpaceAround
    }

    public enum FrameAlignment
    {
        None,
        Start,
        End,
        Center,
        Stretch
    }

    public enum FrameGrid
    {
        /// <summary>
        /// Fill grid with two evenly-sized columns
        /// </summary>
        FillTwo,
        CollapsibleFillTwo
    }

    public enum FrameSpacing
    {
        None,
        Panel,
        Card,
        Line,
        Small,
        Medium,
        Large
    }

    public enum FrameTemplate
    {
        ButtonContainer,
        Panel,
        Card
    }
    public enum FrameBorder
    {
        None,
        Panel,
        Card
    }

    public static class FrameExtensions
    {
        extension(FrameSpacing spacing)
        {
            public Optional<string> GapCssClass => spacing switch
            {
                FrameSpacing.Panel => "frame-gap-panel",
                FrameSpacing.Card => "frame-gap-card",
                FrameSpacing.Line => "frame-gap-line",
                FrameSpacing.Small => "frame-gap-small",
                FrameSpacing.Medium => "frame-gap-medium",
                FrameSpacing.Large => "frame-gap-large",
                _ => new Optional<string>()
            };

            public Optional<string> PaddingCssClass => spacing switch
            {
                FrameSpacing.Panel => "frame-padding-panel",
                FrameSpacing.Card => "frame-padding-card",
                FrameSpacing.Line => "frame-padding-line",
                FrameSpacing.Small => "frame-padding-small",
                FrameSpacing.Medium => "frame-padding-medium",
                FrameSpacing.Large => "frame-padding-large",
                _ => new Optional<string>()
            };
        }

        extension(FrameDirection direction)
        {
            public Optional<string> CssClass => direction switch
            {
                FrameDirection.Col => "frame-flex-col",
                FrameDirection.Row => "frame-flex-row",
                _ => new Optional<string>()
            };
        }

        extension(FrameBorder border)
        {
            public Optional<string> CssClass => border switch
            {
                FrameBorder.Panel => "frame-border-panel",
                FrameBorder.Card => "frame-border-card",
                _ => new Optional<string>()
            };
        }

        extension(FrameJustification justification)
        {
            public Optional<string> CssClass => justification switch
            {
                FrameJustification.Start => "frame-justify-content-flex-start",
                FrameJustification.End => "frame-justify-content-flex-end",
                FrameJustification.Center => "frame-justify-content-center",
                FrameJustification.SpaceAround => "frame-justify-content-space-around",
                FrameJustification.SpaceBetween => "frame-justify-content-space-between",
                _ => new Optional<string>()
            };
        }

        extension(FrameAlignment alignment)
        {
            public Optional<string> CssClass => alignment switch
            {
                FrameAlignment.Start => "frame-align-items-flex-start",
                FrameAlignment.End => "frame-align-items-flex-end",
                FrameAlignment.Center => "frame-align-items-center",
                FrameAlignment.Stretch => "frame-align-items-stretch",
                _ => new Optional<string>()
            };
        }

        extension(FrameGrid grid)
        {
            public Optional<string> CssClass => grid switch
            {
                FrameGrid.FillTwo => "frame-grid-fill-2",
                FrameGrid.CollapsibleFillTwo => "frame-grid-collapsible-fill-2",
                _ => new Optional<string>()
            };
        }
    }
}
