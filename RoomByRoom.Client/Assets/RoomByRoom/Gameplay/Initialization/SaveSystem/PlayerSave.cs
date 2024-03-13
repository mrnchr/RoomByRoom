using System;

namespace RoomByRoom
{
  [Serializable]
  public struct PlayerSave
  {
    public RaceInfo Race;
    public Health HealthCmp;
    public Movable MovableCmp;
    public Jumpable JumpableCmp;
    public UnitPhysicalProtection UnitPhysProtectionCmp;
  }
}