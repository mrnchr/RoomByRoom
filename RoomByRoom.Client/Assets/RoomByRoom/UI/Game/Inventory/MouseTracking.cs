using System.Collections.Generic;
using Leopotam.EcsLite;
using RoomByRoom.Utility;
using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
  public class MouseTracking : MonoBehaviour
  {
    [SerializeField] private List<SlotEventContainer> _slotEvents;
    [SerializeField] private Can _can;
    private EcsWorld _message;
    private GameMediator _mediator;
    private ItemDragger _itemDragger;

    public void Construct(EcsWorld message)
    {
      _message = message;
      _itemDragger = FindObjectOfType<ItemDragger>();
    }

    private void OnEnable()
    {
      _slotEvents.ForEach(x =>
      {
        x.OnMouseEnter += EnterMouse;
        x.OnMouseExit += ExitMouse;
        x.OnMouseDragBegun += BeginDrag;
        x.OnMouseDrag += Drag;
        x.OnMouseDragEnd += EndDrag;
      });

      _can.OnMouseEnter += EnterMouse;
      _can.OnMouseExit += ExitMouse;
    }

    private void OnDisable()
    {
      _slotEvents.ForEach(x =>
      {
        x.OnMouseEnter -= EnterMouse;
        x.OnMouseExit -= ExitMouse;
        x.OnMouseDragBegun -= _itemDragger.BeginDrag;
        x.OnMouseDrag -= _itemDragger.Drag;
        x.OnMouseDragEnd -= _itemDragger.EndDrag;
      });

      _can.OnMouseEnter -= EnterMouse;
      _can.OnMouseExit -= ExitMouse;
    }

    private void EndDrag(Slot slot) => _itemDragger.EndDrag(slot);
    private void Drag(Slot slot) => _itemDragger.Drag(slot);
    private void BeginDrag(Slot slot) => _itemDragger.BeginDrag(slot);

    private void EnterMouse(Slot slot)
    {
      _itemDragger.EnterMouse(slot);
      SendUpdateItemInfoMessage(slot);
    }

    private void ExitMouse(Slot slot)
    {
      _itemDragger.ExitMouse(slot);
      SendEmptyUpdateItemInfoMessage();
    }

    private void SendUpdateItemInfoMessage(Slot slot) =>
      _message.Add<UpdateItemInfoMessage>(_message.NewEntity())
        .Item = slot.Item;

    private void SendEmptyUpdateItemInfoMessage() =>
      _message.Add<UpdateItemInfoMessage>(_message.NewEntity())
        .Item = null;

    private void EnterMouse(Can can) => _itemDragger.EnterMouse(can);
    private void ExitMouse(Can can) => _itemDragger.ExitMouse(can);
  }
}