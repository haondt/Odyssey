using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Odyssey.Domain.Core.Services
{
    public class ComponentStringRenderer(HtmlRenderer htmlRenderer) : IComponentStringRenderer
    {
        public Task<string> RenderComponentAsync(IComponent component, Type componentType)
        {
            return htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var dictionary = component.ToDictionary();
                var parameterView = ParameterView.FromDictionary(dictionary);
                var output = await htmlRenderer.RenderComponentAsync(componentType, parameterView);
                // preemptively adding this for when we figure out how to compress the responses
                var noise = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                return output.ToHtmlString() + $"<!-- noise__{noise} -->";
            });
        }

        public Task<string> RenderComponentAsync<T>() where T : IComponent, new() => RenderComponentAsync(new T());

        public Task<string> RenderComponentAsync<T>(T component) where T : IComponent => RenderComponentAsync(component, typeof(T));

        public Task<string> RenderComponentAsync(IComponent component) => RenderComponentAsync(component, component.GetType());
    }

    // stolen with love from Haondt.Web.Core
    internal static class ComponentExtensions
    {
        private static ConcurrentDictionary<Type, PropertyInfo[]> _parameterCache = new();

        public static Dictionary<string, object?> ToDictionary<T>(this T component) where T : class, IComponent
        {
            if (typeof(T) == typeof(IComponent))
                return ToDictionary(component, component.GetType());
            return ToDictionary(component, typeof(T));
        }
        public static Dictionary<string, object?> ToDictionary(this IComponent component, Type componentType)
        {
            var parameters = _parameterCache.GetOrAdd(componentType,
                t => t.GetProperties()
                    .Where(p => p.GetCustomAttribute<ParameterAttribute>() != null)
                    .ToArray());

            return parameters
                .ToDictionary(p => p.Name, p => p.GetValue(component));
        }
    }

}
