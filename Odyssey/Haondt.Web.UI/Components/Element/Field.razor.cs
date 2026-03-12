using Haondt.Core.Models;

namespace Haondt.Web.UI.Components.Element
{
    public enum FieldType
    {
        /// <summary>
        /// Text input.
        /// </summary>
        Text,
        /// <summary>
        /// Checkbox (boolean) input.
        /// </summary>
        Checkbox,
        /// <summary>
        /// Password input.
        /// </summary>
        Password,
        /// <summary>
        /// Search input.
        /// </summary>
        Search,
        /// <summary>
        /// Use a select tag instead of an input. Should be combined with the <see cref="Field.Options"/> or <see cref="Field.Suggestions"/> parameter.
        /// </summary>
        Dropdown,
        /// <summary>
        /// Search input that fires a <a href="https://developer.mozilla.org/en-US/docs/Web/API/HTMLFormElement/submit_event">submit</a> event whenever its contents change.
        /// </summary>
        LiveSearch,
        /// <summary>
        /// Don't add any input to the field. Can be used with <see cref="Field.ChildContent"/> or just to create an empty field.
        /// </summary>
        None,
        /// <summary>
        /// Hidden input
        /// </summary>
        Hidden,
        /// <summary>
        /// Use a textarea instead of an input.
        /// </summary>
        TextArea,
    }
    public enum FieldSize
    {
        /// <summary>
        /// Fill the horizontal space.
        /// </summary>
        Fill,
        /// <summary>
        /// Fill the horizontal and vertical space.
        /// </summary>
        Full
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

    public record struct SuggestionSource
    {
        public required string Uri { get; set; }
        public Optional<(string Text, Optional<string> Value)> Selected { get; set; }
    }
    public record struct Suggestion
    {
        public required string Text { get; set; }
        public Optional<string> Value { get; set; }

        public static implicit operator Suggestion((string, string) t) => new()
        {
            Text = t.Item1,
            Value = t.Item2
        };
        public static implicit operator Suggestion(string s) => new() { Text = s };
    }
}
