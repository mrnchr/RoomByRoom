using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using RoomByRoom.UI.Game;
using RoomByRoom.Utility;
using UnityEngine;
using static RoomByRoom.WeaponType;
using Object = UnityEngine.Object;

namespace RoomByRoom
{
  public class EquipService
  {
    private readonly EcsWorld _world;
    private readonly CharacteristicService _charSvc;
    private readonly KeepDirtyService _keepDirtySvc;

    public EquipService(EcsWorld world, CharacteristicService charSvc, KeepDirtyService keepDirtySvc)
    {
      _world = world;
      _charSvc = charSvc;
      _keepDirtySvc = keepDirtySvc;
    }

    public void ChangeEquip(int item)
    {
      if (_world.Has<Equipped>(item))
        UnEquip(item);
      else
        Equip(item);
    }

    private void Equip(int item)
    {
      int player = Utils.GetPlayerEntity(_world);
      var inventory = _world.Get<Inventory>(player).ItemList;
      var equipment = _world.Get<Equipment>(player).ItemList;
      var backpack = _world.Get<Backpack>(player).ItemList;
      Debug.Log($"Equip {item}");
      if (Utils.IsMeleeWeapon(_world, item))
      {
        int old = GetMeleeEquipment(equipment);
        if (IsHands(old)) Disarm(old, equipment, inventory);

        if (IsMainWeapon(player, old))
          ChangeMainWeapon(old, item, player);
      }

      _world.Add<Equipped>(item);
      backpack.Remove(_world.PackEntity(item));
      equipment.Add(_world.PackEntity(item));
      _charSvc.Calculate(player);

      _keepDirtySvc.UpdateDirtyMessage(DirtyType.Slots);
    }

    private void UnEquip(int item)
    {
      Debug.Log($"UnEquip {item}");

      int player = Utils.GetPlayerEntity(_world);
      var inventory = _world.Get<Inventory>(player).ItemList;
      var equipment = _world.Get<Equipment>(player).ItemList;
      var backpack = _world.Get<Backpack>(player).ItemList;

      _world.Del<Equipped>(item);
      equipment.Remove(_world.PackEntity(item));
      backpack.Add(_world.PackEntity(item));
      _charSvc.Calculate(player);

      if (Utils.IsMeleeWeapon(_world, item))
      {
        int replace = GetMeleeEquipment(equipment);
        if (replace == -1)
        {
          replace = _world.Filter<NotVisible>().End().GetRawEntities()[0];
          Arm(replace, equipment, inventory);
        }

        if (IsMainWeapon(player, item))
          ChangeMainWeapon(item, replace, player);
      }

      if (_world.Get<ItemInfo>(item).Type == ItemType.Armor)
      {
        DeleteArmorAnimation(item, player);
        DeleteItem(item);
      }

      _keepDirtySvc.UpdateDirtyMessage(DirtyType.Slots);
    }

    private void DeleteArmorAnimation(int armor, int player)
    {
      if (_world.Get<ItemInfo>(armor).Type != ItemType.Armor) return;
      ArmorType armorType = _world.Get<ArmorInfo>(armor).Type;
      if (armorType != ArmorType.Helmet && armorType != ArmorType.Shield)
        GetHumanoidUnit(player).RemoveArmorAnimator(((ArmorView)_world.Get<ItemViewRef>(armor).Value).Anim);
    }

    private HumanoidView GetHumanoidUnit(int player) =>
      (HumanoidView)_world.Get<UnitViewRef>(player).Value;

    private void DeleteItem(int item)
    {
      if (!_world.Has<ItemViewRef>(item)) return;
      GameObject itemObject = _world.Get<ItemViewRef>(item).Value.gameObject;
      itemObject.SetActive(false);
      Object.Destroy(itemObject);
      _world.Del<ItemViewRef>(item);

      _keepDirtySvc.UpdateDirtyMessage(DirtyType.PlayerModel);
    }

    private int GetMeleeEquipment(List<EcsPackedEntity> equipment)
    {
      foreach (int eq in equipment.Select(_world.Unpack).Where(x => Utils.IsMeleeWeapon(_world, x)))
        return eq;
      return -1;
    }

    private void ChangeMainWeapon(int from, int to, int unit)
    {
      DeleteItem(from);
      _world.Del<InHands>(from);

      _world.Add<InHands>(to);
      _world.Get<MainWeapon>(unit)
        .Entity = to;

      Utils.SetWeaponToAnimate(_world, to, unit);
    }

    private bool IsMainWeapon(int unit, int weapon) => _world.Get<MainWeapon>(unit).Entity == weapon;

    private void Arm(int item, List<EcsPackedEntity> equipment, List<EcsPackedEntity> inventory)
    {
      _world.Del<NotVisible>(item);
      _world.Add<Equipped>(item);
      equipment.Add(_world.PackEntity(item));
      inventory.Add(_world.PackEntity(item));
    }

    private void Disarm(int item, List<EcsPackedEntity> equipment, List<EcsPackedEntity> inventory)
    {
      _world.Del<Equipped>(item);
      _world.Add<NotVisible>(item);
      equipment.Remove(_world.PackEntity(item));
      inventory.Remove(_world.PackEntity(item));
    }

    private bool IsHands(int item) =>
      _world.Has<WeaponInfo>(item) && _world.Get<WeaponInfo>(item).Type == None;
  }
}