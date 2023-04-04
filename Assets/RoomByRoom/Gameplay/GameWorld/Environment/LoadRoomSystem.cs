using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using RoomByRoom.Utility;

namespace RoomByRoom
{
    internal class LoadRoomSystem : IEcsInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            int room = world.NewEntity();
            RoomEntity roomEntity = _savedData.Value.Room;

            world.AddComponent<RaceInfo>(room)
                .Initialize(x => x = roomEntity.Race);

            world.AddComponent<RoomInfo>(room)
                .Initialize(x => x = roomEntity.Info);
        }
    }
}