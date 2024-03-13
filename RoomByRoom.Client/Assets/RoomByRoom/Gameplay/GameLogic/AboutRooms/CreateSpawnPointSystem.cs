using System;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class CreateSpawnPointSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<AddPlayerCommand, RoomViewRef>> _rooms = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _rooms.Value)
      {
        _world.Get<RoomViewRef>(index).Value.SpawnPoints
          .ToList().ForEach(CreateSpawnPointEntity);
      }
    }

    private void CreateSpawnPointEntity(SpawnPoint spawn) => _world.Add<SpawnPoint>(_world.NewEntity()) = spawn;
  }
}