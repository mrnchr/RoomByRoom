using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.Config.Data
{
  [CreateAssetMenu(menuName = "RoomByRoom/Data/PrefabData")]
  public class PrefabData : ScriptableObject
  {
    public PlayerView BasePlayerUnit;
    public UnitView[] SandEnemyUnits;
    public UnitView[] WaterEnemyUnits;
    public UnitView[] DarkEnemyUnits;
    public UnitView[] BossUnits;
    public RoomView StartRoom;
    public RoomView[] EnemyRooms;
    public RoomView[] BossRooms;
    public WeaponView PlayerHand;
    public WeaponView[] OneHands;
    public WeaponView[] TwoHands;
    public WeaponView[] Bows;
    public ArmorView[] Boots;
    public ArmorView[] Leggings;
    public ArmorView[] Gloves;
    public ArmorView[] Breastplates;
    public ArmorView[] Helmets;
    public ArmorView[] Shields;
    public ArtifactView[] Artifacts;
    public BonusView Bonus;
  }
}