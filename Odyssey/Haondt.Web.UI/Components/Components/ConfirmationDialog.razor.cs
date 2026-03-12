using Haondt.Web.UI.Components.Element;
using System.ComponentModel.DataAnnotations;

namespace Haondt.Web.UI.Components.Components
{
    public record ConfirmationDialogModel
    {
        [Required]
        [MaxLength(500)]
        public required string Message { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(20)]
        public string? CancelText { get; set; }

        [MaxLength(20)]
        public string? ConfirmText { get; set; }

        public ConfirmationDialogIntent? Intent { get; set; }

        /// <summary>
        /// Note this does not include <see cref="Message"/>
        /// </summary>
        public Dictionary<string, string> DataFields
        {
            get
            {
                var d = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(Title))
                    d["data-confirmation-dialog-title"] = Title;
                if (!string.IsNullOrEmpty(CancelText))
                    d["data-confirmation-dialog-cancel-text"] = CancelText;
                if (!string.IsNullOrEmpty(ConfirmText))
                    d["data-confirmation-dialog-confirm-text"] = ConfirmText;
                if (Intent.HasValue)
                    d["data-confirmation-dialog-intent"] = Intent.Value.ToString();
                return d;
            }
        }

    }

    public enum ConfirmationDialogIntent
    {
        Create,
        Destroy,
        Modify,
        Inert
    }

    public static class ConfirmationDialogExtensions
    {
        extension(ConfirmationDialogIntent intent)
        {
            public ButtonColor ButtonColor => intent switch
            {
                ConfirmationDialogIntent.Create => ButtonColor.Primary,
                ConfirmationDialogIntent.Destroy => ButtonColor.Danger,
                ConfirmationDialogIntent.Modify => ButtonColor.Success,
                _ => ButtonColor.Text,
            };
        }
    }
}
