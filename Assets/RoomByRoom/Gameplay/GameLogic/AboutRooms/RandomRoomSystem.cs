using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class RandomRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<StartGameMessage>> _startGame = Idents.Worlds.MessageWorld;

        public void Run(IEcsSystems systems)
        {
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

            foreach(var index in _startGame.Value)
            {
                // Send NextRoomMessage
                int entity = message.NewEntity();
                ref NextRoomMessage nextRoom = ref message.GetPool<NextRoomMessage>().Add(entity);
                nextRoom.Race.Race = FastRandom.GetRandomEnemyRace();
                nextRoom.Type.Type = RoomType.Enemy;
            }
        }
    }
}