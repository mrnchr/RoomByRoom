using System;

namespace RoomByRoom.UI.Game
{
  [Flags]
  public enum DirtyType
  {
    Slots = 1 << 0,
    PlayerModel = 1 << 1,
    RoomCount = 1 << 2
  }
}