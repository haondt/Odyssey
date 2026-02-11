namespace Haondt.Web.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LayoutOptionsAttribute : Attribute
    {
        public bool FillPage { get; init; } = false;
    }
}
