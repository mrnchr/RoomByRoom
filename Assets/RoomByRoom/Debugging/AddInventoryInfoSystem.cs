using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;

namespace RoomByRoom.Debugging
{
  public class AddInventoryInfoSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<UnitViewRef>, Exc<DebugInfo>> _units = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _units.Value)
      {
        var inv = _world.Get<UnitViewRef>(index)
          .Value.gameObject.AddComponent<DebugInventory>();

        inv.Construct(_world);

        inv.Equipment = _world.Get<Equipment>(index).ItemList;

        if (Utils.IsPlayer(_world, index))
        {
          inv.Backpack = _world.Get<Backpack>(index).ItemList;
          inv.Inventory = _world.Get<Inventory>(index).ItemList;
        }

        _world.Add<DebugInfo>(index).Inv = inv;
      }
    }
  }
}