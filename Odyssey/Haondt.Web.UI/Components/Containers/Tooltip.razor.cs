namespace Haondt.Web.UI.Components.Containers
{
    public enum TooltipAlignment
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public enum TooltipPosition
    {
        Below,
        Above,
        LeftOf,
        RightOf
    }
    public enum TooltipStyle
    {
        Default,
        ContextMenu
    }

    public static class TooltipExtensions
    {
        extension(TooltipStyle style)
        {
            public string CssClass => style switch
            {
                TooltipStyle.ContextMenu => "tooltip-style-context-menu",
                _ => "",
            };
        }
        extension(TooltipAlignment align)
        {
            public string CssClass => align switch
            {
                TooltipAlignment.Top => "tooltip-align-top",
                TooltipAlignment.Bottom => "tooltip-align-bottom",
                TooltipAlignment.Left => "tooltip-align-left",
                TooltipAlignment.Right => "tooltip-align-right",
                _ => "",
            };
        }

        extension(TooltipPosition pos)
        {
            public string CssClass => pos switch
            {
                TooltipPosition.Above => "tooltip-position-above",
                TooltipPosition.Below => "tooltip-position-below",
                TooltipPosition.LeftOf => "tooltip-position-left-of",
                TooltipPosition.RightOf => "tooltip-position-right-of",
                _ => "",
            };
        }
    }
}
