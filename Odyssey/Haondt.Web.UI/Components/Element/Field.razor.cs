using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum FieldType
    {
        Text,
        Checkbox,
        Password,
        Search,
        Dropdown
    }
    public enum FieldSize
    {
        Fill
    }
    public enum FieldAutocomplete
    {
        Username,
        NewPassword,
        CurrentPassword
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
        extension(FieldAutocomplete autocomplete)
        {
            public string StringValue => autocomplete switch
            {
                FieldAutocomplete.Username => "username",
                FieldAutocomplete.NewPassword => "new-password",
                FieldAutocomplete.CurrentPassword => "current-password",
                _ => autocomplete.ToString()
            };
        }
    }

    public record struct DropdownOption
    {
        public required string Text { get; set; }
        public Optional<string> Value { get; set; }
        public bool Selected { get; set; }
    }
}
