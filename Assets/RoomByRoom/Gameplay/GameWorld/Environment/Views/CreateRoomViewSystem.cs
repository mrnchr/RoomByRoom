using System;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    internal class CreateRoomViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PackedPrefabData> _injectPrefabData = default;
        private EcsFilterInject<Inc<RoomInfo, RaceInfo>, Exc<RoomViewRef>> _rooms = default;
        private PrefabData _prefabData;
        private EcsWorld _world;

        public void Run(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _prefabData = _injectPrefabData.Value.Prefabs;

            foreach(var index in _rooms.Value)
            {
                RoomView.InstantiateView(SelectRoom(index), out RoomView roomView);
                roomView.Entity = index;

                _world.AddComponent<RoomViewRef>(index)
                    .Initialize(x => { x.Value = roomView; return x; });

                _world.AddComponent<AddPlayerCommand>(index);
            }
        }

        private GameObject SelectRoom(int room)
        {
            RoomType type = _world.GetComponent<RoomInfo>(room).Type;
            RaceType race = _world.GetComponent<RaceInfo>(room).Type;

            RoomView roomView;
            switch (type)
            {
                case RoomType.Enemy:
                    roomView = _prefabData.EnemyRooms[FastRandom.GetEnemyRoom(_prefabData.EnemyRooms.Length)];
                    break;
                case RoomType.Boss:
                    roomView = _prefabData.BossRooms[(int)race - 1];
                    break;
                default:
                    roomView = _prefabData.StartRoom;
                    break;
            }

            return roomView.gameObject;
        }
    }
}