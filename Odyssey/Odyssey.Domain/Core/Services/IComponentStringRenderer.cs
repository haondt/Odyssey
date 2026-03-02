using Microsoft.AspNetCore.Components;

namespace Odyssey.Domain.Core.Services
{
    public interface IComponentStringRenderer
    {
        Task<string> RenderComponentAsync<T>() where T : IComponent, new();
        Task<string> RenderComponentAsync<T>(T component) where T : IComponent;
        Task<string> RenderComponentAsync(IComponent component);
        Task<string> RenderComponentAsync(IComponent component, Type componentType);
    }
}
