using UnityEngine;

namespace RoomByRoom.Config.Data
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/PlayerData")]
  public class PlayerData : ScriptableObject
  {
    public ArmorInfo Armor;
    public AnimationInfo Animation;
    public float AttackDelay;
    
    [Header("Player")]
    public float TakeItemDistance;
    public int BackpackSize;
    public int EquipmentSize;
  }
}