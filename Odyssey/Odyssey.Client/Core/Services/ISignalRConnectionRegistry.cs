using Haondt.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace Odyssey.Client.Core.Services
{
    public interface ISignalRConnectionRegistry<T> where T : notnull
    {
        void Register(string id, T connection);
        bool TryGetValue(string id, [NotNullWhen(true)] out T? value);
        Optional<T> Unregister(string id);
    }
}
