namespace Odyssey.UI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LayoutOptionsAttribute : Attribute
    {
        public bool ShowNavigationBar { get; init; } = true;
        public bool FillPage { get; init; } = false;
    }
}
