using UnityEngine;

namespace RoomByRoom
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/PlayerData")]
  public class PlayerData : ScriptableObject
  {
    public ArmorData Armor;
    
    [Header("Player")]
    public Movable MovableCmp;
    public Jumpable JumpableCmp;
    public float TakeItemDistance;
    public int BackpackSize;
    public int EquipmentSize;
  }
}