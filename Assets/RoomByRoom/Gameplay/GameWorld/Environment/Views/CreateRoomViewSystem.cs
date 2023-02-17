using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    internal class CreateRoomViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PackedPrefabData> _prefabData = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<RoomInfo, RaceInfo>, Exc<RoomViewRef>> _rooms = default;
        private PackedPrefabData _packedData;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _packedData = _prefabData.Value;

            foreach(var index in _rooms.Value)
            {
                GameObject room;
                RoomType type = _rooms.Pools.Inc1.Get(index).Type;
                RaceType race = _rooms.Pools.Inc2.Get(index).Type;

                // Room Spawn
                room = SpawnRoom(race, type);
                RoomView roomView = room.GetComponent<RoomView>();

                // Add RoomViewRef component
                ref RoomViewRef roomRef = ref world.GetPool<RoomViewRef>().Add(index);
                roomRef.Value = roomView;

                // Add NoPlayer component
                ref NoPlayer noPlayer = ref world.GetPool<NoPlayer>().Add(index);

                // Update scene data
                _sceneData.Value.CurrentRoomEntity = index;
            }
        }

        private GameObject SpawnRoom(RaceType race, RoomType type)
        {
            GameObject room;
            if (type != RoomType.Start)
            {
                if (type == RoomType.Enemy)
                {
                    // There are several rooms of each race
                    // Create the room for the certain race and a random index 
                    room = UnityEngine.Object.Instantiate(_packedData.EnemyRooms[race][UnityEngine.Random.Range(0, _packedData.EnemyRooms[race].Length)].gameObject);
                }
                // Boss Room
                else
                {
                    if(_packedData.Prefabs.BossRooms.Length < Enum.GetNames(typeof(RaceType)).Length - 1)
                        throw new OverflowException("Array length is less then appropriate enum length");
                    room = UnityEngine.Object.Instantiate(_packedData.Prefabs.BossRooms[(int)race - 1].gameObject);
                }
            }
            // Start Room
            else
            {
                room = UnityEngine.Object.Instantiate(_packedData.Prefabs.StartRoom.gameObject);
            }

            return room;
        }
    }
}