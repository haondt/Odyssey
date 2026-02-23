using Haondt.Core.Models;

namespace Odyssey.UI.Core.Exceptions
{
    public class NotFoundToastException : ToastException
    {
        public NotFoundToastException() : base("Not found")
        {
        }
        public NotFoundToastException(string? message) : base(message ?? "Not found")
        {
        }

        public NotFoundToastException(string? message, Exception? innerException) : base(message ?? "Not found", innerException)
        {
        }
        public override Optional<int> StatusCode { get; set; } = 404;
    }
}
