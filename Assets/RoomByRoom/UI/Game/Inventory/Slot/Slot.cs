using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace RoomByRoom.UI.Game.Inventory
{
  public class Slot : MonoBehaviour
  {
    [HideInInspector] public bool IsEmpty;
    public EcsPackedEntity? Item;
    public SlotInfo Info { get; private set; }
    [field: SerializeField] public Image ItemImage { get; private set; }

    public void Construct()
    {
      Info = GetComponent<SlotInfo>();
      IsEmpty = true;
    }

    public void SetItem(Sprite itemSprite = null, ItemInfoForSlot itemInfo = null)
    {
      IsEmpty = !itemSprite;
      Item = itemInfo?.ItemEntity;
      if (itemInfo != null)
        Info.Set(itemInfo.Type, itemInfo.EqType);

      ItemImage.gameObject.SetActive(itemSprite);
      ItemImage.sprite = itemSprite;
    }

    public static void Change(Slot from, Slot to)
    {
      if (!from || !to) return;

      bool tempIsEmpty = to.IsEmpty;
      var tempItem = to.Item;
      ItemType tempType = to.Info.Type;
      int tempEqType = to.Info.EqType;
      Sprite tempSprite = to.ItemImage.sprite;

      to.IsEmpty = from.IsEmpty;
      to.Item = from.Item;
      to.Info.Set(from.Info.Type, from.Info.EqType);
      to.ItemImage.gameObject.SetActive(true);
      to.ItemImage.sprite = from.ItemImage.sprite;

      if (tempIsEmpty)
      {
        from.SetItem();
        return;
      }

      from.IsEmpty = tempIsEmpty;
      from.Item = tempItem;
      from.Info.Set(tempType, tempEqType);
      from.ItemImage.gameObject.SetActive(!tempIsEmpty);
      from.ItemImage.sprite = tempSprite;
    }

    public bool IsMatch(Slot match) => Info.Type == match.Info.Type && Info.SlotEqType == match.Info.SlotEqType;

    public bool HasHands() => Info.IsEquipped && Info.Type == ItemType.Weapon && Info.EqType == (int)WeaponType.None;
  }
}