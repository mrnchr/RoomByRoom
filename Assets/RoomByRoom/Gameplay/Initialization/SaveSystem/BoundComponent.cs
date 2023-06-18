using System;
using UnityEngine.Serialization;

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