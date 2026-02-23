namespace Odyssey.Persistence.Models
{
    public readonly record struct OwnedEntityId<TEntityId>(string OwnerId, TEntityId EntityId);

    public static class OwnedEntityIdExtensions
    {
        extension(OwnedEntityId<Guid> id)
        {
            public string StringValue => $"{id.OwnerId}+{id.EntityId}";
        }
    }
}
