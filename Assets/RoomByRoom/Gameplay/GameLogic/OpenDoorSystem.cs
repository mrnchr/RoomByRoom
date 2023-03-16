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
        private EcsCustomInject<SavedData> _savedData = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);
            EcsWorld world = systems.GetWorld();
            ref GameInfo startGame = ref _savedData.Value.GameInfo;

            foreach (var index in _openDoor.Value)
            {
                // Check pressing open door button and is opener the player 
                foreach(var opener in _opener.Value)
                {
                    // Checking has the game started
                    // If not then to create gameInfo and message about game start
                    if(startGame.RoomCount == 0)
                    {
                        // Send StartGameMessage
                        int startGameEntity = message.NewEntity();
                        message.GetPool<StartGameMessage>().Add(startGameEntity);
                    }

                    // Send NextRoomMessage
                    int nextRoomEntity = message.NewEntity();
                    ref NextRoomMessage nextRoom = ref message.GetPool<NextRoomMessage>().Add(nextRoomEntity);
                    nextRoom.Race.Type = FastRandom.GetEnemyRace();
                    nextRoom.Type = CalculateRoomType(startGame.RoomCount);

                    ++startGame.RoomCount;
                }
            }
        }

        private RoomType CalculateRoomType(int number)
        {
            return number % 10 == 9 ? RoomType.Boss : RoomType.Enemy;
        }
    }
}