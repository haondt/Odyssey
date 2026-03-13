using Odyssey.Core.Models;
using Odyssey.GrainInterfaces.Core.Services;

namespace Odyssey.GrainInterfaces.Sessions.Services
{
    public class SessionGrainFactory<TBoard, TGameState>(IGrainFactory grainFactory) : ISessionGrainFactory<TBoard, TGameState>
        where TBoard : IDataStorageData<TBoard>
        where TGameState : IDataStorageData<TGameState>
    {
        public ISessionGrain<TBoard, TGameState> GetGrain(OwnedEntityGuid key)
        {
            return grainFactory.GetGrain<ISessionGrain<TBoard, TGameState>>(key.EntityId, key.OwnerId);
        }

        public OwnedEntityGuid GetIdentity(ISessionGrain<TBoard, TGameState> grain)
        {
            var id = grain.GetPrimaryKey(out var extension);
            return (extension ?? "", id);
        }
    }
}
