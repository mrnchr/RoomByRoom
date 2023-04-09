using System;
using UnityEngine.Serialization;

namespace RoomByRoom
{
    [Serializable]
    public struct PlayerEntity
    {
        public RaceInfo Race;
        public Health HealthCmp;
        [FormerlySerializedAs("MovingCmp")] public Movable MovableCmp;
        [FormerlySerializedAs("JumpingCmp")] public Jumpable JumpableCmp;
    }
}