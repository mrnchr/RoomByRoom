using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
using RoomByRoom.Utility;

namespace RoomByRoom
{
  internal class LoadPlayerSystem : IEcsInitSystem
  {
    private readonly EcsCustomInject<PlayerData> _playerData = default;
    private readonly EcsCustomInject<Saving> _savedData = default;

    public void Init(IEcsSystems systems)
    {
      EcsWorld world = systems.GetWorld();
      SavedPlayer savedPlayer = _savedData.Value.Player;

      int player = world.NewEntity();
      world.Add<RaceInfo>(player)
        .Assign(x => savedPlayer.Race);

      world.Add<Health>(player)
        .Assign(x => savedPlayer.HealthCmp);

      world.Add<UnitInfo>(player)
        .Type = UnitType.Player;

      world.Add<Inventory>(player)
        .ItemList = new List<EcsPackedEntity>(_playerData.Value.BackpackSize + _playerData.Value.EquipmentSize);

      world.Add<Equipment>(player)
        .ItemList = new List<EcsPackedEntity>(_playerData.Value.EquipmentSize);

      world.Add<Backpack>(player)
        .ItemList = new List<EcsPackedEntity>(_playerData.Value.BackpackSize);

      world.Add<UnitPhysicalProtection>(player) = savedPlayer.UnitPhysProtectionCmp
        .Assign(x =>
        {
          x.RestoreSpeed = _playerData.Value.Armor.RestoreSpeed;
          x.CantRestoreTime = _playerData.Value.Armor.BreakRestoreTime;
          return x;
        });

      world.Add<MainWeapon>(player)
        .Entity = -1;
      world.Add<ControllerByPlayer>(player);
    }
  }
}