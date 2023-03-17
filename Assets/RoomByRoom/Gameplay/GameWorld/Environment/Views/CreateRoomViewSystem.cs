using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    internal class CreateRoomViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PackedPrefabData> _injectPrefabData = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<RoomInfo, RaceInfo>, Exc<RoomViewRef>> _rooms = default;
        private PrefabData _prefabData;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _prefabData = _injectPrefabData.Value.Prefabs;

            foreach(var index in _rooms.Value)
            {
                RoomType type = _rooms.Pools.Inc1.Get(index).Type;
                RaceType race = _rooms.Pools.Inc2.Get(index).Type;

                // Room Spawn
                GameObject room = GameObject.Instantiate(SelectRoom(race, type));
                RoomView roomView = room.GetComponent<RoomView>();
                roomView.Entity = index;

                // Add RoomViewRef component
                ref RoomViewRef roomRef = ref world.GetPool<RoomViewRef>().Add(index);
                roomRef.Value = roomView;

                // Add AddPlayerCommand component
                ref AddPlayerCommand addPlayerCommand = ref world.GetPool<AddPlayerCommand>().Add(index);

                // Update scene data
                _sceneData.Value.CurrentRoomEntity = index;
            }
        }

        private GameObject SelectRoom(RaceType race, RoomType type)
        {
            RoomView room;
            // Enemy room
            if (type == RoomType.Enemy)
            {
                room = _prefabData.EnemyRooms[UnityEngine.Random.Range(0, _prefabData.EnemyRooms.Length)];
            }
            // Boss room
            else if(type == RoomType.Boss)
            {
                if(_prefabData.BossRooms.Length < Enum.GetNames(typeof(RaceType)).Length - 1)
                    throw new OverflowException("Array length is less then appropriate enum length");
                room = _prefabData.BossRooms[(int)race - 1];
            }
            // Start room
            else
            {
                room = _prefabData.StartRoom;
            }

            return room.gameObject;
        }
    }
}