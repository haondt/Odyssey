using Haondt.Core.Models;

namespace Odyssey.UI.Library.Components.Element
{
    public enum FrameDirection
    {
        Col,
        Row
    }

    public enum FrameJustification
    {
        Start,
        End,
        Center,
        SpaceBetween,
        SpaceAround
    }

    public enum FrameAlignment
    {
        Start,
        End,
        Center
    }

    public enum FrameGrid
    {
        /// <summary>
        /// Fill grid with two evenly-sized columns
        /// </summary>
        FillTwo,
        CollapsibleFillTwo
    }

    public enum FrameGap
    {
        Small,
        Medium,
        Large,
        ContentSmall,
        ContentMedium,
        ContentLarge
    }

    public static class FrameExtensions
    {
        extension(FrameGap gap)
        {
            public Optional<string> CssClass => gap switch
            {
                FrameGap.Small => "frame-gap-s",
                FrameGap.Medium => "frame-gap-m",
                FrameGap.Large => "frame-gap-l",
                FrameGap.ContentSmall => "frame-gap-content-s",
                FrameGap.ContentMedium => "frame-gap-content-m",
                FrameGap.ContentLarge => "frame-gap-content-l",
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
