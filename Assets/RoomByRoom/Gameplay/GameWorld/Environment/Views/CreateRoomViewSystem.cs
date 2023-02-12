using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace RoomByRoom
{
    internal class CreateRoomViewSystem : IEcsRunSystem
    {
        private EcsCustomInject<PrefabData> _prefabDataInject = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsFilterInject<Inc<RoomInfo, RaceInfo>, Exc<RoomViewRef>> _rooms = default;
        private PrefabData _prefabData;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _prefabData = _prefabDataInject.Value;

            foreach(var index in _rooms.Value)
            {
                GameObject room;
                RoomType type = _rooms.Pools.Inc1.Get(index).Type;
                RaceType race = _rooms.Pools.Inc2.Get(index).Race;

                // Room Spawn
                room = SpawnRoom(race, type);
                RoomView roomView = room.GetComponent<RoomView>();

                // Add SpawnPoint component
                ref var spawnPoint = ref world.GetPool<SpawnPoint>().Add(index);
                spawnPoint.PlayerSpawn = roomView.SpawnPoint.position;

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
                    // There is several enemy room variants for each race
                    // Because we need to find the array of certain race
                    RoomView[] roomArray = PickRoomArray(race);
                    room = UnityEngine.Object.Instantiate(roomArray[UnityEngine.Random.Range(0, roomArray.Length)].gameObject);
                }
                // Boss Room
                else
                {
                    if(_prefabData.BossRooms.Length < Enum.GetNames(typeof(RaceType)).Length - 1)
                        throw new OverflowException("Array length is less then appropriate enum length");
                    room = UnityEngine.Object.Instantiate(_prefabData.BossRooms[(int)race - 1].gameObject);
                }
            }
            // Start Room
            else
            {
                room = UnityEngine.Object.Instantiate(_prefabData.StartRoom.gameObject);
            }

            return room;
        }

        private RoomView[] PickRoomArray(RaceType race)
        {
            if(race == RaceType.Dark)
            {
                return _prefabData.DarkEnemyRooms;
            }
            else if(race == RaceType.Sand)
            {
                return _prefabData.SandEnemyRooms;
            }
            else if(race == RaceType.Water)
            {
                return _prefabData.WaterEnemyRooms;
            }
            else {
                throw new ArgumentException();
            }
        }
    }
}