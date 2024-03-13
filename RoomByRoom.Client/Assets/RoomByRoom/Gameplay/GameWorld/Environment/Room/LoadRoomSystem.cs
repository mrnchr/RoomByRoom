using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  internal class LoadRoomSystem : IEcsInitSystem
  {
    private readonly EcsCustomInject<ProgressData> _gameSave = default;

    public void Init(IEcsSystems systems)
    {
      EcsWorld world = systems.GetWorld();
      RoomSave roomSave = _gameSave.Value.Room;

      int room = world.NewEntity();
      world.Add<RaceInfo>(room) = roomSave.Race;
      world.Add<RoomInfo>(room) = roomSave.Info;
    }
  }
}