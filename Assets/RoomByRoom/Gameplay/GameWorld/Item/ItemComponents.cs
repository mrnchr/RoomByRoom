using System;

namespace RoomByRoom
{
  [Serializable]
  public struct ItemInfo
  {
    public ItemType Type;
  }

  public struct ItemViewRef
  {
    public ItemView Value;
  }

  [Serializable]
  public struct ShapeInfo
  {
    public int PrefabIndex;
  }
}