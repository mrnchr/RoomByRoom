using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using RoomByRoom.Utility;

namespace RoomByRoom
{
    public class RecreateRoomSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
        private EcsCustomInject<SceneData> _sceneData = default;

        public void Run(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            EcsWorld message = systems.GetWorld(Idents.Worlds.MessageWorld);

            foreach(var index in _nextRoom.Value)
            {
                // Delete current room
                Object.Destroy(world.GetPool<RoomViewRef>().Get(_sceneData.Value.CurrentRoomEntity).Value.gameObject);
                world.DelEntity(_sceneData.Value.CurrentRoomEntity);
                
                // Create new room
                int roomEntity = world.NewEntity();
                ref NextRoomMessage nextRoom = ref _nextRoom.Pools.Inc1.Get(index);

                // Add RaceInfo component
                ref RaceInfo race = ref world.GetPool<RaceInfo>().Add(roomEntity);
                race = nextRoom.Race;

                // Add RoomInfo component
                ref RoomInfo room = ref world.GetPool<RoomInfo>().Add(roomEntity);
                room = nextRoom.Type;
            }
        }
    }
}