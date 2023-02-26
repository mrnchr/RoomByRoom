using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom.Debug
{
    public class RemoveEnemySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<UnitViewRef, CanBeDeleted>> _enemies = default;
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;

        public void Run(IEcsSystems systems)
        {
            // Delete marked (old) enemies when new room is spawning

            EcsWorld world = systems.GetWorld();

            foreach(var nextRoom in _nextRoom.Value)
            {
                foreach(var enemy in _enemies.Value)
                {
                    ref UnitViewRef unitRef = ref _enemies.Pools.Inc1.Get(enemy);
                    UnityEngine.GameObject.Destroy(unitRef.Value.gameObject);
                    world.DelEntity(enemy);
                }
            }
        }
    }
}
