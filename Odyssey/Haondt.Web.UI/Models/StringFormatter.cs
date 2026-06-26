using System.Text.RegularExpressions;
using Haondt.Core.Models;

namespace Haondt.Web.UI.Models
{
    public partial class StringFormatter
    {

        public static string PascalToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return PascalCaseParser().Replace(value, "-").ToLower();
        }

        [GeneratedRegex(@"(?<!^)(?=[A-Z][a-z])|(?<=[a-z])(?=[A-Z])")]
        private static partial Regex PascalCaseParser();

        [GeneratedRegex(@"[^A-Za-z0-9\-]")]
        private static partial Regex NonSlugCharacterParser();

        [GeneratedRegex(@"-{2,}")]
        private static partial Regex RepeatedDashParser();

        // TODO: localize this properly
        public static string FormatDate(AbsoluteDateTime dateTime)
        {
            return dateTime.LocalTime.ToString("yyyy-MM-dd");
        }

        public static string Slugify(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var replaced = NonSlugCharacterParser().Replace(value, "-");
            var collapsed = RepeatedDashParser().Replace(replaced, "-");
            return collapsed.Trim('-');
        }

        public static string PascalCaseToSlug(string value)
        {
            return Slugify(PascalToKebabCase(value));
        }
    }
}
