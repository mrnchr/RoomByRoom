using RoomByRoom.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.UI.Game.Inventory
{
  public class SlotInfo : MonoBehaviour
  {
    [field: SerializeField] public bool IsEquipped { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [SerializeField] private SlotWeaponType _slotWeaponType;
    [SerializeField] private ArmorType _armorType;
    private WeaponType _weaponType;

    public int EqType
    {
      get => Type == ItemType.Weapon ? (int)_weaponType : (int)_armorType;
      private set
      {
        if (Type == ItemType.Weapon)
          _weaponType = (WeaponType)value;
        else
          _armorType = (ArmorType)value;
      }
    }

    public int SlotEqType => Type == ItemType.Weapon ? (int)_slotWeaponType : (int)_armorType;

    public void Set(ItemType type, int eqType)
    {
      if (IsEquipped)
      {
        if (Utils.IsMeleeWeapon(type, eqType))
          EqType = eqType;

        return;
      }

      Type = type;
      EqType = eqType;
      _slotWeaponType = Utils.IsMeleeWeapon(type, eqType) ? SlotWeaponType.Melee : SlotWeaponType.Bow;
    }
  }

  public enum SlotWeaponType
  {
    Melee = 0,
    Bow = 1
  }
}