using Microsoft.Extensions.DependencyInjection;

namespace Odyssey.Domain.Core.Services
{
    public class EventTransformerRegistry(IServiceProvider serviceProvider) : IEventTransformerRegistry
    {
        public T GetTransformer<T>() where T : IEventTransformer
        {
            return serviceProvider.GetRequiredService<T>();
        }
        public T GetTransformer<T>(object key) where T : IEventTransformer
        {
            return serviceProvider.GetRequiredKeyedService<T>(key);
        }
    }
}
