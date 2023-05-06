using System;

namespace RoomByRoom
{
  public struct AddPlayerCommand
  {
  }

  [Serializable]
  public struct RoomInfo
  {
    public RoomType Type;
  }

  public struct RoomViewRef
  {
    public RoomView Value;
  }
}