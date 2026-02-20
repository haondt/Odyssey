using Haondt.Core.Models;
using System.Text.RegularExpressions;

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

        // TODO: localize this properly
        public static string FormatDate(AbsoluteDateTime dateTime)
        {
            return dateTime.LocalTime.ToString("yyyy-MM-dd");
        }
    }
}
