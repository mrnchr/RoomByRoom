using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
    /// <summary>
    /// Remove marked (old) enemies when new room is spawning
    /// </summary>
    public class RemoveEnemySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<UnitViewRef, CanBeDeleted>> _enemies = default;
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            foreach(var index1 in _nextRoom.Value)
            {
                foreach(var index2 in _enemies.Value)
                {
                    ref UnitViewRef unitRef = ref _enemies.Pools.Inc1.Get(index2);
                    UnityEngine.GameObject.Destroy(unitRef.Value.gameObject);
                    world.DelEntity(index2);
                }
            }
        }
    }
}
