using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class CreateEnemySystem : IEcsRunSystem
    {
        EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;

        public void Run(IEcsSystems systems)
        {
        }
    }
}