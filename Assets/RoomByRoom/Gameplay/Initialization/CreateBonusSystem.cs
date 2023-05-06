using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  public class CreateBonusSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<DieCommand, UnitViewRef>> _units = default;
    private readonly EcsCustomInject<PrefabService> _prefabData = default;
    private readonly EcsCustomInject<GameInfo> _gameInfo = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _units.Value)
      {
        int bonus = _world.NewEntity();
        _world.Add<Bonus>(bonus)
          .Item = FastRandom.CreateItem(_world, _prefabData.Value, _gameInfo.Value);
        _world.Add<SpawnCommand>(bonus)
          .Coords = _world.Get<UnitViewRef>(index).Value.transform.position;
      }
    }
  }
}