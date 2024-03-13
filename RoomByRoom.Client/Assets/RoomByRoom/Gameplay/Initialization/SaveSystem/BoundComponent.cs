using System;

namespace RoomByRoom
{
  [Serializable]
  public struct BoundComponent<T>
    where T : struct
  {
    public int Entity;
    public T ComponentInfo;
  }
}