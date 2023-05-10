using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using RoomByRoom.Config.Data;
using RoomByRoom.Utility;
using Rand = UnityEngine.Random;

namespace RoomByRoom
{
  public class WearHumanoidEnemySystem : IEcsRunSystem
  {
    private readonly EcsCustomInject<GameInfo> _gameInfo = default;
    private readonly EcsCustomInject<PrefabService> _prefabSvc = default;
    private readonly EcsFilterInject<Inc<Bare>> _units = default;
    private EcsWorld _world;

    public void Run(IEcsSystems systems)
    {
      _world = systems.GetWorld();

      foreach (int index in _units.Value)
      {
        CreateArmors(index);
        CreateWeapon(index);
      }
    }

    private void CreateArmors(int entity)
    {
      int maxArmorNumber = Utils.GetEnumLength<ArmorType>() - 1;
      int armorNumber = Rand.Range(0, maxArmorNumber);
      var armorTypes = new List<int>(new[] { 0, 1, 2, 3, 4 });

      for (var i = 0; i < armorNumber; i++)
      {
        int index = Rand.Range(0, armorTypes.Count);
        CreateArmor((ArmorType)armorTypes[index], entity);

        armorTypes.RemoveAt(index);
      }
    }

    private void CreateArmor(ArmorType armorType, int unit)
    {
      int armor = CreateItemEntity(ItemType.Armor, (int)armorType, unit);

      _world.Add<ArmorInfo>(armor)
        .Type = armorType;

      _world.Add<ItemPhysicalProtection>(armor)
        .Point = FastRandom.GetArmorProtection(armorType, _gameInfo.Value.RoomCount);
    }

    private void CreateWeapon(int unit)
    {
      // TODO: change to random
      var weaponType = WeaponType.OneHand; // FastRandom.GetWeaponType();

      int weapon = CreateItemEntity(ItemType.Weapon, (int)weaponType, unit);

      _world.Add<WeaponInfo>(weapon)
        .Type = weaponType;

      _world.Add<ItemPhysicalDamage>(weapon)
        .Point = FastRandom.GetPhysicalDamage(weaponType, _gameInfo.Value.RoomCount);

      _world.Add<InHands>(weapon);

      _world.Add<MainWeapon>(unit)
        .Entity = weapon;

      // Utils.SetWeaponToAnimate(_world, weapon);

      // if (weaponType == WeaponType.OneHand && Rand.Range(0f, 1f) >= 0.5)
      // 	CreateShield(unit);
    }

    // TODO: shield has persistent rather than protection
    private void CreateShield(int entity) => CreateArmor(ArmorType.Shield, entity);

    private int CreateItemEntity(ItemType itemType, int equipmentType, int unitEntity)
    {
      int item = _world.NewEntity();

      _world.Add<ItemInfo>(item)
        .Type = itemType;

      _world.Add<Equipped>(item);

      _world.Add<Shape>(item)
        .PrefabIndex = FastRandom.GetPrefabIndex(_prefabSvc.Value, itemType, equipmentType);

      _world.Add<Owned>(item)
        .Owner = unitEntity;

      return item;
    }
  }
}