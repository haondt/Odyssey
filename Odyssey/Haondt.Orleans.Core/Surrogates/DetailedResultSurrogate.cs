namespace Haondt.Orleans.Core.Surrogates
{
    [GenerateSerializer]
    public struct DetailedResultSurrogate<TReason>
    {
        [Id(0)]
        public TReason? Reason;
        [Id(1)]
        public bool IsSuccessful;
    }

    [GenerateSerializer]
    public struct DetailedResultSurrogate<T, TReason>
    {
        [Id(0)]
        public TReason? Reason;
        [Id(1)]
        public T? Value;
        [Id(2)]
        public bool IsSuccessful;
    }
}
