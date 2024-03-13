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

    private readonly List<EcsPackedEntity> _dumpedItems = new List<EcsPackedEntity>();

    public void DropItem(Slot slot)
    {
      if (!slot || slot.IsEmpty || slot.Item == null) return;
      
      _dumpedItems.Add(slot.Item.Value);
    }

    public List<EcsPackedEntity> Clear()
    {
      var temp = new List<EcsPackedEntity>(_dumpedItems);
      _dumpedItems.Clear();
      return temp;
    }
  }
}