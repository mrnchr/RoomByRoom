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
            int roomEntity = world.NewEntity();

            // Add RaceInfo component
            ref RaceInfo race = ref world.GetPool<RaceInfo>().Add(roomEntity);
            race = _savedData.Value.RoomRace;

            // Add RoomInfo component
            ref RoomInfo type = ref world.GetPool<RoomInfo>().Add(roomEntity);
            type = _savedData.Value.RoomType;
        }
    }
}