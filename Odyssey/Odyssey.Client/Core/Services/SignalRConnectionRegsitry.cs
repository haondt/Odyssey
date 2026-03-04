using Haondt.Core.Models;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Odyssey.Client.Core.Services
{
    public class SignalRConnectionRegsitry<T> : ISignalRConnectionRegistry<T> where T : notnull
    {
        private readonly ConcurrentDictionary<string, T> _connections = [];

        public void Register(string id, T connection)
        {
            if (_connections.ContainsKey(id))
                throw new ArgumentException($"Connection ID {id} already registered.");
            _connections[id] = connection;
        }

        public Optional<T> Unregister(string id)
        {
            if (_connections.Remove(id, out var connection))
                return connection;
            return new();
        }

        public bool TryGetValue(string id, [NotNullWhen(true)] out T? value) => _connections.TryGetValue(id, out value);
    }
}
