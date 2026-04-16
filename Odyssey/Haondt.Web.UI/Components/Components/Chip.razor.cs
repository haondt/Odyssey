using System.ComponentModel.DataAnnotations;
using Haondt.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Components.Components
{
    public record ChipModel
    {
        [Required]
        [MaxLength(500)]
        public required string Text { get; set; }

        [MaxLength(100)]
        public string? Label { get; set; }

        public bool? Deletable { get; set; }
        public bool? EmitEvents { get; set; }

        public string? InputName { get; set; }
        public string? InputValue { get; set; }
    }

    public record ChipInputData(
        string? Name = default,
        string? Value = default);

    public enum ChipColor
    {
        Text,
        TextWeak,
        Primary,
        Subtitle,
        Border,
        BackgroundStrong,
        ChipBackground
    }

    public enum ChipStyle
    {
        Default,
        Fill,
        Outline
    }

    public enum ChipTextStyle
    {
        Default,
        Monospaced
    }

}
