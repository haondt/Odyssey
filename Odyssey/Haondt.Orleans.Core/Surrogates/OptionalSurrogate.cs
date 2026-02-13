namespace Haondt.Orleans.Core.Surrogates
{
    [GenerateSerializer]
    public struct OptionalSurrogate<T>
    {
        [Id(0)]
        public T? Value;
        [Id(1)]
        public bool HasValue;
    }
}
