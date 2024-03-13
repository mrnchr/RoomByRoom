using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
  public class ItemDragger : MonoBehaviour
  {
    [SerializeField] private Transform _inventory;
    [SerializeField] private GameMediator _mediator;
    private Slot _lastSlot;
    private Slot _newSlot;
    private bool _isDrag;
    private Can _can;
    private Vector3 _lastPosition;
    private InventoryUpdater _inventoryUpdater;

    private void Awake()
    {
      _inventoryUpdater = FindObjectOfType<InventoryUpdater>();
      _mediator = FindObjectOfType<GameMediator>();
    }

    public void BeginDrag(Slot slot)
    {
      _isDrag = false;
      _newSlot = null;
      _can = null;
      if (slot.IsEmpty || slot.HasHands()) return;
      _isDrag = true;
      _lastSlot = slot;
      _lastPosition = slot.ItemImage.transform.position;
      slot.ItemImage.transform.SetParent(_inventory);
      slot.ItemImage.transform.SetAsLastSibling();
    }

    public void Drag(Slot slot)
    {
      if (_isDrag)
        slot.ItemImage.transform.position = Input.mousePosition;
    }

    public void EndDrag(Slot slot)
    {
      if (!_isDrag) return;
      BreakDrag();

      if (_can)
      {
        _can.DropItem(slot);
        if(slot.Info.IsEquipped && slot.Item != null)
          _mediator.ChangeEquip(slot.Item.Value);
        slot.SetItem();
        return;
      }

      if (!_newSlot) return;
      _inventoryUpdater.MoveItem(slot, _newSlot);
    }

    private void ReturnImage(Slot slot)
    {
      slot.ItemImage.transform.position = _lastPosition;
      slot.ItemImage.transform.SetParent(slot.transform);
      slot.ItemImage.transform.SetAsLastSibling();
    }

    public void BreakDrag()
    {
      if (!_isDrag) return;
      ReturnImage(_lastSlot);
      _isDrag = false;
    }

    public void EnterMouse(Slot slot) => _newSlot = slot;
    public void ExitMouse(Slot slot) => _newSlot = null;
    public void EnterMouse(Can can) => _can = can;
    public void ExitMouse(Can can) => _can = null;
  }
}