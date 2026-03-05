namespace Odyssey.Core.Exceptions
{
    public static class ExceptionFactory
    {
        public static ArgumentException CasesExhaustedException<T>(T value)
            => new($"Unknown {typeof(T).Name}, \"{value}\"");
        public static ArgumentException CasesExhaustedException<T>(T value, string name)
            => new($"Unknown {name}: \"{value}\"");
    }
}
