using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class RecreateRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoomMsg = Idents.Worlds.MessageWorld;
        private EcsFilterInject<Inc<RoomViewRef>> _room = default;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

            foreach(var index in _nextRoomMsg.Value)
            {
                DeleteRoom();
                CreateRoom(message.GetComponent<NextRoomMessage>(index));
            }
        }

        private void CreateRoom(NextRoomMessage nextRoom)
        {
            int roomEntity = _world.NewEntity();

            _world.AddComponent<RaceInfo>(roomEntity)
                .Initialize(x => x = nextRoom.Race);

            _world.AddComponent<RoomInfo>(roomEntity)
                .Initialize(x => x = nextRoom.Room);
        }

        private void DeleteRoom()
        {
            int roomEntity = _room.Value.GetRawEntities()[0];
            GameObject.Destroy(_world.GetComponent<RoomViewRef>(roomEntity).Value.gameObject);
            _world.DelEntity(roomEntity);
        }
    }
}