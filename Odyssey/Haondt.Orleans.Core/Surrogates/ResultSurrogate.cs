namespace Haondt.Orleans.Core.Surrogates
{
    [GenerateSerializer]
    public struct ResultSurrogate<T>
    {
        [Id(0)]
        public T? Value;
        [Id(1)]
        public bool IsSuccessful;
    }
}
