using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum FieldType
    {
        Text,
        Checkbox,
        Password,
        Search
    }
    public enum FieldSize
    {
        Fill
    }
    public static class FieldExtensions
    {
        extension(FieldSize size)
        {
            public Optional<string> CssClass => size switch
            {
                FieldSize.Fill => "field-size-fill",
                _ => new Optional<string>()
            };

        }
    }
}
