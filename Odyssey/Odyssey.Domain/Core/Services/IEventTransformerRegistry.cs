namespace Odyssey.Domain.Core.Services
{
    public interface IEventTransformerRegistry
    {
        TTransformer GetTransformer<TTransformer>() where TTransformer : IEventTransformer;
        TTransformer GetTransformer<TTransformer>(object key) where TTransformer : IEventTransformer;
    }
}
