using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom
{
    public class OpenDoorSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<OpenDoorMessage>> _openDoor = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<Opener>> _opener = default;
        private EcsFilterInject<Inc<GameInfo>> _gameInfo = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);
            EcsWorld world = systems.GetWorld();

            foreach (var index in _openDoor.Value)
            {
                // Check pressing open door button and is opener the player 
                if (_opener.Value.GetEntitiesCount() > 0)
                {
                    // Checking has the game started
                    // If not then to create gameInfo and message about game start
                    if(_gameInfo.Value.GetEntitiesCount() == 0)
                    {
                        int gameEntity = world.NewEntity();
                        ref GameInfo gameInfo = ref _gameInfo.Pools.Inc1.Add(gameEntity);
                        gameInfo.RoomCount = 0;

                        // Send StartGameMessage
                        int entity = message.NewEntity();
                        message.GetPool<StartGameMessage>().Add(entity);
                    }
                    // Create message about a new room creating and increment roomCount
                    else 
                    {
                        ref GameInfo gameInfo = ref _gameInfo.Pools.Inc1.Get(_gameInfo.Value.GetRawEntities()[0]);

                        // Send NextRoomMessage
                        int entity = message.NewEntity();
                        ref NextRoomMessage nextRoom = ref message.GetPool<NextRoomMessage>().Add(entity);
                        nextRoom.Race.Race = FastRandom.GetRandomEnemyRace();
                        nextRoom.Type.Type = CalculateRoomType(gameInfo.RoomCount);

                        ++gameInfo.RoomCount;
                    }
                }
            }
        }

        private RoomType CalculateRoomType(int number)
        {
            return number % 10 == 9 ? RoomType.Boss : RoomType.Enemy;
        }
    }
}