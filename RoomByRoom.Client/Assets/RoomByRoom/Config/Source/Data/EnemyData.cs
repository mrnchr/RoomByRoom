using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.Config.Data
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/EnemyData")]
  public class EnemyData : ScriptableObject
  {
    public ArmorInfo Armor;
    public AnimationInfo Animation;
    [FormerlySerializedAs("DelayAttackTime")] public float AttackDelay;

    [Header("Humanoid")]
    public Movable MovableCmp;
    public Jumpable JumpableCmp;
    public float AttackDistance;
  }
}