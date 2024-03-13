using System;
using UnityEngine.Serialization;

namespace RoomByRoom.Config.Data
{
  [Serializable]
  public struct ArmorInfo
  {
    public float RestoreSpeed;
    [FormerlySerializedAs("BreakRestoreTime")] public float CantRestoreTime;
  }
}