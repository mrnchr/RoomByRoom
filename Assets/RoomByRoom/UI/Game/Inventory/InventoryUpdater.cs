using System.Collections.Generic;
using Leopotam.EcsLite;
using RoomByRoom.Config.Data;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
  public class InventoryUpdater : MonoBehaviour
  {
    [SerializeField] private List<Slot> _inventory;
    private EcsWorld _world;
    private EcsWorld _message;
    private SpriteService _spriteSvc;
    private GameMediator _mediator;

    public void Construct(EcsWorld world, EcsWorld message, SpriteService spriteSvc)
    {
      _world = world;
      _message = message;
      _spriteSvc = spriteSvc;
      _mediator = FindObjectOfType<GameMediator>();

      _inventory.ForEach(x => x.Construct());
    }

    public void CleanAll() => _inventory.ForEach(x => x.SetItem());

    public void AddItem(ItemInfoForSlot info)
    {
      Slot slot = GetSlot(info.Type, info.EqType, info.IsEquipped);

      // Debug.Log($"index: {_world.Unpack(info.ItemEntity)}, type: {info.Type}, eqType: {info.EqType}");
      // Debug.Log(slot ? slot.name : slot);

      slot.SetItem(_spriteSvc.GetItem(info.Type, info.EqType, info.Shape), info);
      _message.Add<InventoryChangedMessage>(_message.NewEntity());
    }

    public void MoveItem(Slot from, Slot to)
    {
      // Debug.Log($"{from.Info.IsEquipped}, {to.Info.IsEquipped}");
      switch (from.Info.IsEquipped, to.Info.IsEquipped)
      {
        case (true, true):
          return;
        case (true, false):
        {
          if (to.IsEmpty)
          {
            Equip(from);
            Change(from, to);
          }
          else if (from.IsMatch(to))
          {
            Equip(from);
            Equip(to);
            Change(from, to);
          }
          else if ((to = _inventory.Find(x => x.IsEmpty && !x.Info.IsEquipped)) != null)
          {
            Equip(from);
            Change(from, to);
          }

          break;
        }
        case (false, true):
        {
          if (from.IsMatch(to))
          {
            if (to.IsEmpty)
            {
              Equip(from);
              Change(from, to);
            }
            else
            {
              if (to.HasHands())
                to.SetItem();
              else
                Equip(to);
              Equip(from);
              Change(from, to);
            }
          }

          break;
        }
        case (false, false):
        {
          if (to.IsEmpty)
            Change(from, to);
          else
            Change(from, to);
          break;
        }
      }
    }

    private void Change(Slot from, Slot to)
    {
      Slot.Change(from, to);
      _mediator.UpdateItemDescription();
      _mediator.UpdateItemRender();
      _message.Add<InventoryChangedMessage>(_message.NewEntity());
    }

    private void Equip(Slot slot) => _mediator.ChangeEquip(_world.Unpack(slot.Item ?? new EcsPackedEntity()));

    private Slot GetSlot(ItemType type, int eqType, bool isEquipped) =>
      isEquipped
        ? _inventory.Find(x =>
                            x.Info.Type == type && GetSlotEquipmentType(x.Info.Type, x.Info.EqType) ==
                            GetSlotEquipmentType(type, eqType))
        : _inventory.Find(x => x.IsEmpty && !x.Info.IsEquipped);

    private int GetSlotEquipmentType(ItemType type, int eqType) =>
      type == ItemType.Weapon && (int)WeaponType.Bow != eqType
        ? (int)WeaponSlotType.Melee
        : eqType;
  }

  public class ItemInfoForSlot
  {
    public EcsPackedEntity ItemEntity;
    public ItemType Type;
    public int EqType;
    public bool IsEquipped;
    public int Shape;
  }
}