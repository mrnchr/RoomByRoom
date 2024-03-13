using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.Utility
{
  public static class Utils
  {
    public static int GetEnumLength<T>()
      where T : Enum =>
      Enum.GetNames(typeof(T)).Length;

    public static bool IsUnitOf(EcsWorld world, int unit, UnitType checkType) =>
      world.Get<UnitInfo>(unit).Type == checkType;

    public static bool IsEnemy(UnitType type) => type != UnitType.Player && type != UnitType.Boss;
    public static bool IsEnemy(EcsWorld world, int unit) => IsEnemy(world.Get<UnitInfo>(unit).Type);

    public static void AddItemToList(List<EcsPackedEntity> list, EcsPackedEntity item)
    {
      if (list.Contains(item))
        throw new ArgumentException("You try to add an item which is in the item list");
      list.Add(item);
    }

    public static float Clamp(this ref float obj, float min = float.MinValue, float max = float.MaxValue)
    {
      if (obj < min)
        obj = min;
      if (obj > max)
        obj = max;
      return obj;
    }

    public static void SetTransform(Transform from, Transform to)
    {
      from.position = to.position;
      from.rotation = to.rotation;
    }

    public static void PutItemInPlace(Transform item, Transform place)
    {
      item.SetParent(place);
      SetTransform(item, place);
    }

    public static void UpdateTimer<T>(EcsWorld world, int entity, float time)
      where T : struct, ITimerable =>
      world.Update<T>(entity).TimeLeft = time;

    public static int GetOwner(EcsWorld world, int item) => world.Get<Owned>(item).Owner;

    public static void SetWeaponToAnimate(EcsWorld world, int unit) =>
      SetWeaponToAnimate(world, world.Get<MainWeapon>(unit).Entity, unit);

    public static void SetWeaponToAnimate(EcsWorld world, int item, int owner)
    {
      var humanoid = (HumanoidView)world.Get<UnitViewRef>(owner).Value;
      humanoid.SetWeaponToAnimate(world.Get<WeaponInfo>(item).Type);
    }

    public static bool IsMeleeWeapon(ItemType type, int eqType) =>
      type == ItemType.Weapon && eqType != (int)WeaponType.Bow;

    public static bool IsMeleeWeapon(EcsWorld world, int item) =>
      IsMeleeWeapon(world.Get<ItemInfo>(item).Type, GetEquipmentType(world, item));

    public static int GetEquipmentType(EcsWorld world, int item) =>
      world.Get<ItemInfo>(item).Type == ItemType.Weapon
        ? (int)world.Get<WeaponInfo>(item).Type
        : (int)world.Get<ArmorInfo>(item).Type;

    public static int GetPlayerEntity(EcsWorld world) => world.Filter<ControllerByPlayer>().End().GetRawEntities()[0];
    public static int GetRoomEntity(EcsWorld world) => world.Filter<RoomInfo>().End().GetRawEntities()[0];
  }
}