using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.Debugging
{
  public class MarkCanDeletedSystem : IEcsRunSystem
  {
    private readonly EcsFilterInject<Inc<Bonus>, Exc<CanBeDeleted>> _bonuses = default;
    private readonly EcsFilterInject<Inc<UnitViewRef>, Exc<ControllerByPlayer, CanBeDeleted>> _enemies = default;
    private readonly EcsFilterInject<Inc<ItemInfo>> _items = default;
    private readonly EcsFilterInject<Inc<NextRoomMessage>> _nextRoom = Idents.Worlds.MessageWorld;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      // Add CanBeDeleted component for all enemies. This component
      // is needed to delete these enemies on the next room's spawn
      // We do it when next room message is out because when next room 
      // message is send and this script is executed new and old
      // enemies are same

      if (_nextRoom.Value.GetEntitiesCount() > 0) return;

      _world = systems.GetWorld();

      foreach (int index in _enemies.Value)
        _world.Add<CanBeDeleted>(index);

      foreach (int index in _items.Value)
      {
        switch (_world.Has<CanBeDeleted>(index), IsPlayerWeapon(index))
        {
          case (true, true):
            _world.Del<CanBeDeleted>(index);
            break;
          case (false, false):
            _world.Add<CanBeDeleted>(index);
            break;
        }
      }

      foreach (int index in _bonuses.Value)
        _world.Add<CanBeDeleted>(index);
    }

    private bool IsPlayerWeapon(int entity)
    {
      if (!_world.Has<Owned>(entity)) 
        return false;
      
      int unit = _world.Get<Owned>(entity).Owner;
      return Utils.IsUnitOf(_world, unit, UnitType.Player);

    }
  }
}