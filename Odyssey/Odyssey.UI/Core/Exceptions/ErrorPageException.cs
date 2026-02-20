using Haondt.Core.Models;

namespace Odyssey.UI.Core.Exceptions
{
    public class ErrorPageException : Exception
    {
        public virtual int StatusCode { get; set; } = 500;
        public Optional<string> Details { get; set; }
        public Optional<string> InternalDetails { get; set; }

        public ErrorPageException()
        {
        }

        public ErrorPageException(string? message) : base(message)
        {
        }

        public ErrorPageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
