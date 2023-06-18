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
    private readonly EcsCustomInject<GameSave> _gameSave = default;

    public void Init(IEcsSystems systems)
    {
      PlayerSave playerSave = _gameSave.Value.Player;
      PlayerData playerData = _playerData.Value;
      EcsWorld world = systems.GetWorld();

      int player = world.NewEntity();
      world.Add<RaceInfo>(player)
        .Assign(x => playerSave.Race);

      world.Add<Health>(player)
        .Assign(x => playerSave.HealthCmp);

      world.Add<UnitInfo>(player)
        .Type = UnitType.Player;

      world.Add<Inventory>(player)
        .ItemList = new List<EcsPackedEntity>(playerData.BackpackSize + playerData.EquipmentSize);

      world.Add<Equipment>(player)
        .ItemList = new List<EcsPackedEntity>(playerData.EquipmentSize);

      world.Add<Backpack>(player)
        .ItemList = new List<EcsPackedEntity>(playerData.BackpackSize);

      world.Add<UnitPhysicalProtection>(player) = playerSave.UnitPhysProtectionCmp
        .Assign(x =>
        {
          x.RestoreSpeed = playerData.Armor.RestoreSpeed;
          x.CantRestoreTime = playerData.Armor.CantRestoreTime;
          return x;
        });

      world.Add<MainWeapon>(player)
        .Entity = -1;
      world.Add<ControllerByPlayer>(player);
    }
  }
}