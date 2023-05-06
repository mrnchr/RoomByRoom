using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoomByRoom.UI.Game.Inventory
{
  public class SlotEventContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
                                    IDragHandler, IEndDragHandler
  {
    public delegate void MouseEnterHandler(Slot slot);

    public event MouseEnterHandler OnMouseEnter;

    public delegate void MouseExitHandler(Slot slot);

    public event MouseExitHandler OnMouseExit;

    public delegate void BeginMouseDragHandler(Slot slot);

    public event BeginMouseDragHandler OnMouseDragBegun;

    public delegate void MouseDragHandler(Slot slot);

    public event MouseDragHandler OnMouseDrag;

    public delegate void EndMouseDragHandler(Slot slot);

    public event EndMouseDragHandler OnMouseDragEnd;

    private Slot _slot;

    private void Awake()
    {
      _slot = GetComponent<Slot>();
    }

    public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(_slot);
    public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke(_slot);
    public void OnBeginDrag(PointerEventData eventData) => OnMouseDragBegun?.Invoke(_slot);
    public void OnDrag(PointerEventData eventData) => OnMouseDrag?.Invoke(_slot);
    public void OnEndDrag(PointerEventData eventData) => OnMouseDragEnd?.Invoke(_slot);
  }
}