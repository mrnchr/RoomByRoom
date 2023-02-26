using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    public class CreateSpawnPointSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<AddPlayerCommand, RoomViewRef>> _rooms = default;
        private EcsFilterInject<Inc<SpawnPoint>> _spawns = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach (var index in _rooms.Value)
            {
                ref RoomViewRef roomRef = ref _rooms.Pools.Inc2.Get(index);
                SpawnPoint[] spawns = roomRef.Value.SpawnPoints;
                
                foreach(var spawn in spawns)
                {
                    int entity = world.NewEntity();
                    ref SpawnPoint newSpawn = ref _spawns.Pools.Inc1.Add(entity);
                    newSpawn = spawn;
                }
            }
        }
    }
}