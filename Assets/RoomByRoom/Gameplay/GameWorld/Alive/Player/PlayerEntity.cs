using System;

namespace RoomByRoom
{
    [Serializable]
    public struct PlayerEntity
    {
        public RaceInfo Race;
        public Healthy Health;
        public Moving Moving;
        public Jumping Jumping;
    }
}