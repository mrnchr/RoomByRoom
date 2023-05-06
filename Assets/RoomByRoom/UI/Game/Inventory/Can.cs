using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomByRoom.UI.Game.Inventory
{
  public class Can : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public delegate void MouseEnterHandler(Can can);

    public event MouseEnterHandler OnMouseEnter;

    public delegate void MouseExitHandler(Can can);

    public event MouseExitHandler OnMouseExit;

    public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(this);
    public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke(this);

    private AboutSlot _lastSlot;
    private List<EcsPackedEntity> _dumpedItems = new List<EcsPackedEntity>();

    public void DropItem(Slot slot)
    {
      if (!slot || slot.IsEmpty) return;
      _lastSlot = new AboutSlot
      {
        Slot = slot,
        Sprite = slot.ItemImage.sprite,
        Item = slot.Item ?? new EcsPackedEntity(),
        Type = slot.Info.Type,
        EqType = slot.Info.EqType
      };
      _dumpedItems.Add(_lastSlot.Item);
    }

    public void RestoreItem()
    {
      _dumpedItems.Remove(_lastSlot.Item);
    }

    public List<EcsPackedEntity> Clear()
    {
      var temp = new List<EcsPackedEntity>(_dumpedItems);
      _dumpedItems.Clear();
      return temp;
    }
  }

  public class AboutSlot
  {
    public Slot Slot;
    public Sprite Sprite;
    public EcsPackedEntity Item;
    public ItemType Type;
    public int EqType;
  }
}