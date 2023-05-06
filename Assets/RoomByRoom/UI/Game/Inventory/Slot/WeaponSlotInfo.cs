using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
  public class WeaponSlotInfo : MonoBehaviour
  {
    [SerializeField] private WeaponSlotType _type;
    public int Type => (int)_type;
  }

  public enum WeaponSlotType
  {
    Melee = 0,
    Bow = 1
  }
}