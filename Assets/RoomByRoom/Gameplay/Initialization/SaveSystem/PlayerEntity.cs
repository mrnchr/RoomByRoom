using System;

namespace RoomByRoom
{
    [Serializable]
    public struct PlayerEntity
    {
        public RaceInfo Race;
        public Health HealthCmp;
        public Moving MovingCmp;
        public Jumping JumpingCmp;
    }
}