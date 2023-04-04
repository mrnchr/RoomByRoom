using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
    public class MarkEnemySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<UnitViewRef>, Exc<ControllerByPlayer, CanBeDeleted>> _enemies = default;

        public void Run(IEcsSystems systems)
        {
            // Add CanBeDeleted component for all enemies. This component
            // is needed to delete these enemies on the next room's spawn
            // We do it when next room message is out because when next room 
            // message is sended and this script is executed new and old
            // enemies are same

            if(_nextRoom.Value.GetEntitiesCount() > 0)
                return;

            EcsWorld world = systems.GetWorld();

            foreach(var enemy in _enemies.Value)
            {
                world.AddComponent<CanBeDeleted>(enemy);
            }
        }
    }
}