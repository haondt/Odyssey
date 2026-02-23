using Haondt.Core.Models;
using Haondt.Web.UI.Components.Element;

namespace Odyssey.UI.Core.Exceptions
{
    public class ToastException : Exception
    {
        public virtual Optional<int> StatusCode { get; set; }
        public ToastSeverity Severity { get; set; } = ToastSeverity.Error;
        public Optional<string> Title { get; set; }

        public ToastException()
        {
        }

        public ToastException(string? message) : base(message)
        {
        }

        public ToastException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
