namespace Odyssey.Persistence.Models
{
    public readonly record struct OwnedEntityId<TEntityId>(string OwnerId, TEntityId EntityId)
    {
    }
    public readonly record struct OwnedEntityGuid(string OwnerId, Guid EntityId)
    {
        public static implicit operator string(OwnedEntityGuid id) => $"{id.OwnerId}+{id.EntityId}";
        public static implicit operator OwnedEntityGuid((string, Guid) t) => new(t.Item1, t.Item2);
    }

}
