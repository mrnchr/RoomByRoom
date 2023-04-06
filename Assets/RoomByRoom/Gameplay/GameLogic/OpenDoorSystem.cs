using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class OpenDoorSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<OpenDoorMessage>> _openDoorMsg = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<Opener>> _opener = default;
        private EcsCustomInject<GameInfo> _gameInfo = default;
        private EcsWorld _message;

        public void Run(IEcsSystems systems)
        {
            _message = systems.GetWorld(Idents.Worlds.MessageWorld);

            foreach (var index in _openDoorMsg.Value)
            {
                foreach (var _ in _opener.Value)
                {
                    if (IsFirstRoom(_gameInfo.Value.RoomCount))
                        StartGame();

                    CreateNextRoom();

                    ++_gameInfo.Value.RoomCount;
                }
            }
        }

        private void CreateNextRoom()
        {
            _message.AddComponent<NextRoomMessage>(_message.NewEntity())
                .Initialize(x =>
                {
                    x.Race.Type = FastRandom.EnemyRace;
                    x.Room.Type = GetRoomType(_gameInfo.Value.RoomCount);
                    return x;
                });
        }

        private void StartGame()
        {
            int startGameEntity = _message.NewEntity();
            _message.AddComponent<StartGameMessage>(startGameEntity);
        }

        private static bool IsFirstRoom(int number)
        {
            return number == 0;
        }

        private static RoomType GetRoomType(int number)
        {
            return number % 10 == 9 ? RoomType.Boss : RoomType.Enemy;
        }
    }
}