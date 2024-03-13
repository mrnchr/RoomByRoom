using UnityEngine;

namespace RoomByRoom
{
  public struct Selected
  {
  }

  public struct DeselectCommand
  {
  }

  public struct SelectCommand
  {
  }

  public struct SpawnCommand
  {
    public Vector3 Coords;
  }

  public struct BonusViewRef
  {
    public BonusView Value;
  }

  public struct Bonus
  {
    public int Item;
  }
}