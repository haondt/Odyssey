namespace Odyssey.UI.Core.Exceptions
{
    public class NotFoundErrorPageException : ErrorPageException
    {
        public NotFoundErrorPageException() : base("Not found")
        {
        }
        public NotFoundErrorPageException(string? message) : base(message ?? "Not found")
        {
        }

        public NotFoundErrorPageException(string? message, Exception? innerException) : base(message ?? "Not found", innerException)
        {
        }
        public override int StatusCode { get; set; } = 404;
    }
}
