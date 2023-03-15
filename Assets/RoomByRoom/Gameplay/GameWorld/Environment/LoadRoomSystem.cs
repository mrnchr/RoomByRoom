using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class LoadRoomSystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            // Create saved room
            int room = world.NewEntity();
            RoomEntity roomEntity = _savedData.Value.Room;

            // Add RaceInfo component
            ref RaceInfo race = ref world.GetPool<RaceInfo>().Add(room);
            race = roomEntity.Race;

            // Add RoomInfo component
            ref RoomInfo type = ref world.GetPool<RoomInfo>().Add(room);
            type = roomEntity.Info;
        }
    }
}